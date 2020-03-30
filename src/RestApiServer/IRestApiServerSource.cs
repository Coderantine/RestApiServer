using System.Collections.Generic;

namespace RestApiServer
{
    internal interface IRestApiServerSource
    {
        IDictionary<string, IEnumerable<RestSet>> RestSets { get; }
    }
}
