using Npgsql;
using System.Data;
using WebAPI_Giris.Services.OtherServices.Interfaces;

namespace WebAPI_Giris.Services.OtherServices
{
    public class DbService : IDbService
    {
        readonly IConfiguration configuration;
        private NpgsqlConnection conn;
        private string connString;

        public DbService(IConfiguration configuration)
        {
            this.configuration = configuration;
            connString = this.configuration["ConnectionStrings:default"];
            conn = new NpgsqlConnection(connString);
        }

        public NpgsqlConnection GetConnection()
        {
            return conn;
        }

        public async Task OpenConnectionAsync()
        {
            try
            {
                await conn.OpenAsync();
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.StackTrace);
            }
        }

        public async Task CheckConnectionAsync()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    if (conn.State != ConnectionState.Executing)
                    {
                        await conn.OpenAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.StackTrace);
            }

        }

        public void CloseConnection()
        {
            try
            {
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.StackTrace);
            }
        }
    }
}
