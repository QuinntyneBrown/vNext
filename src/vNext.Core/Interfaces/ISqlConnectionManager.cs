using System.Data.SqlClient;


namespace vNext.Core.Interfaces
{
    public interface ISqlConnectionManager
    {
        SqlConnection GetConnection();
    }
}
