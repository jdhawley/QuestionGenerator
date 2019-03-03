using QuestionGenerator.Data;
using QuestionGenerator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionGenerator.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            using (LocalDbContext context = new LocalDbContext())
            {
                if(context.Questions.Count() == 0)
                {
                    SeedData.SeedQuestions(@"C:\Users\Jonathan\source\repos\QuestionGenerator\QuestionGenerator.Data\ProcessedSeedData\BookOfQuestions.txt");
                    SeedData.SeedQuestions(@"C:\Users\Jonathan\source\repos\QuestionGenerator\QuestionGenerator.Data\ProcessedSeedData\QuestionsForCouples.txt");
                }

                List<Question> randomQuestions = context.Questions
                                                        .Where(q => q.DateUsedUTC == null)
                                                        .OrderByDescending(q => q.PreferredQuestion)
                                                        .ThenBy(r => Guid.NewGuid())
                                                        .Take(3)
                                                        .ToList();

                foreach(Question question in randomQuestions)
                {
                    Console.WriteLine(question.QuestionText);
                }
            }



            //TODO: Pull 3 random questions
            //TODO: Send questions to Katie and I

            Console.WriteLine("Execution complete. Press enter to close.");
            Console.ReadLine();
        }
    }
}
