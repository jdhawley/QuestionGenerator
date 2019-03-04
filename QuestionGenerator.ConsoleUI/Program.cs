using QuestionGenerator.Data;
using QuestionGenerator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using RestSharp.Authenticators;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace QuestionGenerator.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            List<string> mailgunTo = new List<string>();
            string mailgunApiKey = configuration.GetSection("mailgun-apikey").Value;
            string mailgunDomain = configuration.GetSection("mailgun-domain").Value;
            string mailgunFrom = configuration.GetSection("mailgun-from").Value;
            configuration.GetSection("mailgun-to").Bind(mailgunTo);

            using (LocalDbContext context = new LocalDbContext())
            {
                if (context.Questions.Count() == 0)
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

                string questions = "";
                foreach (Question question in randomQuestions)
                {
                    questions += question.QuestionText + "\n";
                }

                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                                mailgunApiKey);
                RestRequest request = new RestRequest();
                request.AddParameter("domain", mailgunDomain, ParameterType.UrlSegment);
                request.Resource = $"{mailgunDomain}/messages";
                request.AddParameter("from", mailgunFrom);
                foreach (string emailTo in mailgunTo)
                    request.AddParameter("to", emailTo);
                request.AddParameter("subject", $"Email Questions for {DateTime.Today.ToString("MM/dd/yyyy")}");
                request.AddParameter("text", questions);
                request.Method = Method.POST;
                Console.WriteLine(client.Execute(request));
            }



            //TODO: Update the questions to show they have been sent.

            Console.WriteLine("Execution complete. Press enter to close.");
            Console.ReadLine();
        }
    }
}
