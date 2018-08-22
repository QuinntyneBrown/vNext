using System.Data;
using System.Threading.Tasks;

namespace vNext.Core.Interfaces
{
    public interface IProcedure<TParameters,TResult>
    {
        Task<TResult> ExecuteAsync(TParameters request, IDbConnection connection);
    }
}
