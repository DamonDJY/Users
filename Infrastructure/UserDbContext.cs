using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;

namespace Users.Infrastructure
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistorys { get; set; }
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }

       // public static readonly LoggerFactory LoggerFactory =
       //new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) });
        // 输出到Console
      

        public static readonly ILoggerFactory MyLoggerFactory
    = LoggerFactory.Create(builder => { builder.AddConsole(); });


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }

    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            string strconn = "Data Source=.;Initial Catalog=UserMg;Integrated Security=True; Encrypt=True;TrustServerCertificate=True;";

            var bulider = new DbContextOptionsBuilder<UserDbContext>();
            bulider.UseLoggerFactory(UserDbContext.MyLoggerFactory)
                .UseSqlServer(strconn);

            return new UserDbContext(bulider.Options);

            
        }
    }
}
