using Digital.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Digital.WebApi.RestExtension
{
    public static class DbContextExtension
    {
        public static void AddDbContextExtension(this IServiceCollection services, IConfiguration Configuration)
        {
            var dbType = Configuration.GetConnectionString("DbType");
            if (dbType == "MsSql")
            {
                var dbConfig = Configuration.GetConnectionString("MsSqlConnection");
                services.AddDbContext<DigitalEfDbContext>(opts =>
                opts.UseSqlServer(dbConfig));
            }
            else if (dbType == "PostgreSql")
            {
                var dbConfig = Configuration.GetConnectionString("PostgreSqlConnection");
                services.AddDbContext<DigitalEfDbContext>(opts =>
                  opts.UseNpgsql(dbConfig));
            }
        }
    }
}
