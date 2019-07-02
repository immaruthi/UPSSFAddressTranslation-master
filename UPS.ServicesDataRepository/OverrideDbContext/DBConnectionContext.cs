using Microsoft.Extensions.Configuration;

namespace UPS.ServicesDataRepository.OverrideDbContext
{
    public class DBConnectionContext
    {
        public static string connectionString { get; set; }

        public static string getconnection(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            return connectionString;
        }
    }
}
