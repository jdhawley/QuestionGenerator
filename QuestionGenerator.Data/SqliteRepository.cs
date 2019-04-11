using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Domain;

namespace QuestionGenerator.Data
{
    public class SqliteRepository : IRepository
    {
        public QuestionDbContext _context { get; set; }

        public SqliteRepository(QuestionDbContext context)
        {
            _context = context;
            _context.Database.Migrate();
        }

        public void AddQuestion(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        public List<Question> GetRandomQuestions(int numQuestions)
        {
            return _context.Questions
                           .Where(q => q.DateUsedUTC == null)
                           .OrderByDescending(q => q.PreferredQuestion)
                           .ThenBy(r => Guid.NewGuid())
                           .Take(3)
                           .ToList();
        }

        public int QuestionCount()
        {
            return _context.Questions.Count();
        }

        public void UpdateDateUsed(Question question)
        {
            _context.Questions.Update(question);
            _context.SaveChanges();
        }

        public List<Question> SearchQuestions(string searchText)
        {
            return _context.Questions.Where(x => x.QuestionText.Contains(searchText)).ToList();
        }
    }
}
