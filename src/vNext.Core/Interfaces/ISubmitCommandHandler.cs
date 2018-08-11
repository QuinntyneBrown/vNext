using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace vNext.Core.Interfaces
{
    public interface ISubmitCommandHandler
    {
        Task<TResponse> Handle<TRquest, TResponse>(TRquest request, Func<TRquest, SqlConnection, Task<TResponse>> procedure, string domain, int id, int version, SqlConnection connection);
    }
}
