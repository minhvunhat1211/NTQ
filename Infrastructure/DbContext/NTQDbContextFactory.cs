using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DbContext
{
    public class NTQDbContextFactory : IDesignTimeDbContextFactory<NTQDbContext>
    {
        public NTQDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppConfig.json")
                .Build();

            var connectionString = configuration.GetConnectionString("NTQPrac");


            var optionsBuilder = new DbContextOptionsBuilder<NTQDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new NTQDbContext(optionsBuilder.Options);
        }
    }
}
