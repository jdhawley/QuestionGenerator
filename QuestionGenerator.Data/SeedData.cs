using QuestionGenerator.Domain;

namespace QuestionGenerator.Data
{
    public class SeedData
    {
        public static void SeedQuestions(IRepository repository, string filepath)
        {
            foreach (string question in System.IO.File.ReadAllLines(filepath))
            {
                repository.AddQuestion(new Question()
                {
                    QuestionText = question,
                    PreferredQuestion = false
                });
            }
        }
    }
}
