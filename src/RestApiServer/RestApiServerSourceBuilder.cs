using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace RestApiServer
{
    internal class RestApiServerSourceBuilder : IRestApiServerSourceBuilder
    {
        private readonly List<RestSet> _restSets = new List<RestSet>(1000);

        public IRestApiServerSourceBuilder AddDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            var type = typeof(TDbContext);
            var baseSetMethodINfo = type.GetMethod("Set");
            var dbSetProps = GetDbSetProperties(type);

            foreach (var dbSetProp in dbSetProps)
            {
                var entityType = dbSetProp.PropertyType.GetGenericArguments()[0];
                _restSets.Add(new RestSet()
                {
                    Name = dbSetProp.Name,
                    SetMethodInfo = baseSetMethodINfo.MakeGenericMethod(entityType),
                    EntityType = entityType,
                    DbContextType = type,
                });
            }

            return this;
        }

        public IRestApiServerSource Build()
        {
            return new RestApiServerSource(_restSets);
        }

        private static IEnumerable<PropertyInfo> GetDbSetProperties(Type type)
        {
            foreach (var property in type.GetProperties())
            {
                var setType = property.PropertyType;
                var isDbSet = setType.IsGenericType && typeof(DbSet<>).IsAssignableFrom(setType.GetGenericTypeDefinition());
                if (isDbSet)
                {
                    yield return property;
                }
            }
        }
    }
}
