using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using QuestionGenerator.Data;
using System;

namespace QuestionGenerator.ConsoleUI
{
    public class QuestionDbContextFactory : IDesignTimeDbContextFactory<QuestionDbContext>
    {
        private string connectionString;

        public QuestionDbContext CreateDbContext(string[] args)
        {
            if(connectionString == null)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
                connectionString = configuration.GetConnectionString("QuestionDatabase");
            }

            var optionsBuilder = new DbContextOptionsBuilder<QuestionDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new QuestionDbContext(optionsBuilder.Options);
        }
    }
}
