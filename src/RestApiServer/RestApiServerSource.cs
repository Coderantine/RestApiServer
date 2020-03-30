using System;
using System.Collections.Generic;
using System.Linq;

namespace RestApiServer
{
    internal class RestApiServerSource : IRestApiServerSource
    {
        public RestApiServerSource(IEnumerable<RestSet> restSets)
        {
            RestSets = restSets
                .GroupBy(x => x.DbContextType.Name, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(k => k.Key, v => v.AsEnumerable());
        }

        public IDictionary<string, IEnumerable<RestSet>> RestSets { get; private set; }
    }
}
