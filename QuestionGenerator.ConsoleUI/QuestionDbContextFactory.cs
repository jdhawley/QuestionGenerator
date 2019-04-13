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

        public QuestionDbContextFactory(IConfigurationRoot configuration)
        {
            connectionString = configuration.GetConnectionString("QuestionDatabase");
        }

        public QuestionDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuestionDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new QuestionDbContext(optionsBuilder.Options);
        }
    }
}
