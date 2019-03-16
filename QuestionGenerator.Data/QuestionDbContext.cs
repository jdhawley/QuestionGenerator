using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Domain;

namespace QuestionGenerator.Data
{
    public class QuestionDbContext : DbContext
    {
        private string ConnectionString;
        public QuestionDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public QuestionDbContext()
        { }

        public DbSet<Question> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
    }
}
