using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WebAPI_Giris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IConfiguration _configuration;
        private string sqlDataSource;
        private NpgsqlConnection _connection;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
            sqlDataSource = _configuration.GetConnectionString("default");
            _connection = new NpgsqlConnection(sqlDataSource);

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(closeConnection); //runs on exit
        }







        //SUPPORT METHODS
        [NonAction]
        void closeConnection(object sender, EventArgs e)
        {
            _connection.Close();
        }

        [NonAction]
        public async Task checkConnectionAsync()
        {
            if (_connection.State != ConnectionState.Open) await _connection.OpenAsync();
        }
    }
}
