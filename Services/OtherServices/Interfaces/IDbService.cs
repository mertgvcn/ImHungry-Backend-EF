using Npgsql;

namespace WebAPI_Giris.Services.OtherServices.Interfaces
{
    public interface IDbService
    {
        public NpgsqlConnection GetConnection();
        public Task OpenConnectionAsync();
        public Task CheckConnectionAsync();
        public void CloseConnection();
    }
}
