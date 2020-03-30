using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApiServer
{
    public interface IDbSetExecuter
    {
        Type EntityType { get; }

        ValueTask<object> CreateAsync(object createObject);

        ValueTask<object> GetSingleAsync(string id);

        ValueTask UpdateAsync(string id, object updateObject);

        ValueTask<IEnumerable<object>> GetCollectionAsync();

        ValueTask DeleteAsync(string id);
    }
}
