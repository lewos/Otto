using Microsoft.EntityFrameworkCore;

namespace Otto.models
{
    public class OttoDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        //public DbSet<UserCompany> UsersCompany { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }
        public DbSet<StockRequest> StockRequests { get; set; }
        public DbSet<ProductInStock> ProductsInStock { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //=> options.UseNpgsql($"Server=localhost;Port=5432;Database=postgres;Uid=postgres;Pwd=123456;");
            // options.UseSqlServer("Server=DESKTOP-B30N7LF\\SQLEXPRESS;Database=NewOtto;Trusted_Connection=True;");

            var dbHost = Environment.GetEnvironmentVariable("MSSQL_HOST") ?? "localhost";
            var dbPass = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD")?? "Pa$w0rd2019";
            var dbPID = Environment.GetEnvironmentVariable("MSSQL_PID") ?? "Express";
            var dbName = "Otto";
            Console.WriteLine($"---dbPass--- {dbPass}");

            var connectionStrig = $"Data Source={dbHost}; Initial Catalog={dbName}; User ID=sa;Password={dbPass}";
            options.UseSqlServer(connectionStrig);
        }
    }
}
