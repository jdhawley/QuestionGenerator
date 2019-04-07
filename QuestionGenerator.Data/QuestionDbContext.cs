using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Domain;

namespace QuestionGenerator.Data
{
    public class QuestionDbContext : DbContext
    {
        private readonly string ConnectionString;
        public QuestionDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public QuestionDbContext()
        { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserQuestion> UserQuestions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
    }
}
