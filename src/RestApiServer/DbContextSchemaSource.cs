using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RestApiServer
{
    public class DbContextSchemaCache
    {
        public DbContextSchemaCache(IEnumerable<RestSet> restSets)
        {
            RestSets = restSets;
        }

        public IEnumerable<RestSet> RestSets { get; private set; }
    }

    public static class DbContextSchemaSource
    {
        private static Dictionary<string, DbContextSchemaCache> _cache = new Dictionary<string, DbContextSchemaCache>();

        public static IReadOnlyDictionary<string, DbContextSchemaCache> Cache => _cache;


        public static void Load<TDbContext>() where TDbContext : DbContext
        {
            var type = typeof(TDbContext);
            var baseSetMethodINfo = type.GetMethod("Set");
            var dbSetProps = GetDbSetProperties(type);

            var restSets = new List<RestSet>();
            foreach (var dbSetProp in dbSetProps)
            {
                var entityType = dbSetProp.PropertyType.GetGenericArguments()[0];
                restSets.Add(new RestSet()
                {
                    Name = dbSetProp.Name,
                    SetMethodInfo = baseSetMethodINfo.MakeGenericMethod(entityType),
                    EntityType = entityType,
                });
            }

            _cache.Add(type.Name, new DbContextSchemaCache(restSets));
        }

        private static IEnumerable<PropertyInfo> GetDbSetProperties(Type type)
        {
            foreach (var property in type.GetProperties())
            {
                var setType = property.PropertyType;
                var isDbSet = setType.IsGenericType && (typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition()));
                if (isDbSet)
                {
                    yield return property;
                }
            }
        }
    }


    public class RestSet
    {
        public string Name { get; set; }

        public MethodInfo SetMethodInfo { get; set; }
        public Type EntityType { get; set; }
    }

}
