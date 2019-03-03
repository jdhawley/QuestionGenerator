using QuestionGenerator.Data;
using System;
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
            }

            //TODO: Pull 3 random questions
            //TODO: Send questions to Katie and I

            Console.WriteLine("Execution complete. Press enter to close.");
            Console.ReadLine();
        }
    }
}
