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
                    SeedData.SeedBookOfQuestions(@"C:\Users\Jonathan\source\repos\QuestionGenerator\QuestionGenerator.Data\UnprocessedSeedData\BookOfQuestions.txt");
                    //SeedData.SeedQuestionsForCouples(@"C:\Users\Jonathan\source\repos\QuestionGenerator\QuestionGenerator.Data\UnprocessedSeedData\QuestionsForCouples.txt");
                }
            }

            //TODO: Pull 3 random questions
            //TODO: Send questions to Katie and I
        }
    }
}
