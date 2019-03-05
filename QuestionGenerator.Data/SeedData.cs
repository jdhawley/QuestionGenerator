namespace QuestionGenerator.Data
{
    public class SeedData
    {
        public static void SeedQuestions(QuestionDbContext context, string filepath)
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
