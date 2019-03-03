using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Domain;

namespace QuestionGenerator.Data
{
    public class LocalDbContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //TODO: Find out how to use dependency injection to get the connection string from ConsoleUI.
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=QuestionGenerator;Integrated Security=True");
        }
    }
}
