using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApiServer
{
    public interface IDbSetExecuter
    {
        Type EntityType { get; }
        Task<object> CreateAsync(object createObject);

        Task<object> GetSingleAsync(string id);
        Task<object> UpdateAsync(string id, object updateObject);

        Task<IEnumerable<object>> GetCollectionAsync();

        Task<object> DeleteAsync(string id);
    }
}
