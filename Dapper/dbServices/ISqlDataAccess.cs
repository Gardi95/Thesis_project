using System.Data;

namespace DapperAPI.dbServices
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadMany<T>(string sql);
        Task<T> LoadSingle<T>(string sql);
        Task<bool> insertData(string sql);
        Task<int> insertDataWithReturn(string sql);
        Task<T> insertDataWithObjectReturn<T>(string sql);

    }
}
