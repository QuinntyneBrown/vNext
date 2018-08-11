using System.Data;

namespace vNext.Core.Interfaces
{
    public interface IDbConnectionManager
    {
        IDbConnection GetConnection(string key);
    }
}
