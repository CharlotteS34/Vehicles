using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Vehicles.Repository;

namespace Vehicles.IntegrationTests.Controllers
{
    public abstract class ControllerTestBase : IDisposable
    {
        protected readonly HttpClient _client;
        private readonly DbConnection _connection;
        public ControllerTestBase() 
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddDbContext<VehicleDbContext>(options => options.UseSqlite(_connection));
                    });
                });
            _client = factory.CreateClient();
        }

        public DbConnection Connection => _connection;

        public void Dispose()
        {
            Connection.Close();
        }
    }
}
