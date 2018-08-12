using System;
using System.Data;
using System.Threading.Tasks;

namespace vNext.Core.Interfaces
{
    public interface ISubmitCommandHandler
    {
        Task<TResponse> Handle<TRquest, TResponse>(TRquest request, Func<TRquest, IDbConnection, Task<TResponse>> procedure, string domain, int id, int version, IDbConnection connection);
    }
}
