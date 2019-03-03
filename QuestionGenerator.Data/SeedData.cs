using System;
using System.Text;
using System.Text.RegularExpressions;

namespace QuestionGenerator.Data
{
    public class SeedData
    {
        public static void SeedQuestions(string filepath)
        {
            using (LocalDbContext context = new LocalDbContext())
            {
                foreach (string question in System.IO.File.ReadAllLines(filepath))
                {
                    context.Questions.Add(new Domain.Question()
                    {
                        QuestionText = question,
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
