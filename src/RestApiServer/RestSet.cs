using System;
using System.Reflection;

namespace RestApiServer
{
    internal class RestSet
    {
        public string Name { get; set; }

        public Type DbContextType { get; set; }

        public MethodInfo SetMethodInfo { get; set; }

        public Type EntityType { get; set; }
    }
}
