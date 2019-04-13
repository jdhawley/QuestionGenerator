using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Domain;

namespace QuestionGenerator.Data
{
    public class QuestionDbContext : DbContext
    {
        public QuestionDbContext(DbContextOptions<QuestionDbContext> options) : base(options)
        { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserQuestion> UserQuestions { get; set; }
    }
}
