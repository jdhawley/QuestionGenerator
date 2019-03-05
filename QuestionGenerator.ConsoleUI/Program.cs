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
        private static QuestionDbContext _context;

        static void Main(string[] args)
        {
            IConfigurationRoot configuration = GetConfiguration();
            InitializeDbContext(configuration);
            SeedEmptyDatabase();
            List<Question> randomQuestions = QueryRandomQuestions();
            EmailQuestions(configuration, randomQuestions);

            //TODO: Update the questions to show they have been sent.

            Console.WriteLine("Execution complete. Press enter to close.");
            Console.ReadLine();
        }

        private static void EmailQuestions(IConfigurationRoot configuration, List<Question> randomQuestions)
        {
            RestClient client = new RestClient();
            ConfigureClient(configuration, client);

            RestRequest request = new RestRequest();
            ConfigureRequest(configuration, request, randomQuestions);
            
            client.Execute(request);
        }

        private static void ConfigureRequest(IConfigurationRoot configuration, RestRequest request, List<Question> randomQuestions)
        {
            ConfigureRequestDomain(configuration, request);
            ConfigureRequestTo(configuration, request);
            ConfigureRequestFrom(configuration, request);
            ConfigureRequestSubject(request);
            ConfigureRequestEmail(request, randomQuestions);
            ConfigureRequestMethod(request);
        }

        private static void ConfigureRequestMethod(RestRequest request)
        {
            request.Method = Method.POST;
        }

        private static void ConfigureRequestEmail(RestRequest request, List<Question> randomQuestions)
        {
            string questions = "";
            foreach (Question question in randomQuestions)
            {
                questions += question.QuestionText + "\n";
            }
            request.AddParameter("text", questions);
        }

        private static void ConfigureRequestSubject(RestRequest request)
        {
            request.AddParameter("subject", $"Email Questions for {DateTime.Today.ToString("MM/dd/yyyy")}");
        }

        private static void ConfigureRequestTo(IConfigurationRoot configuration, RestRequest request)
        {
            List<string> mailgunTo = new List<string>();
            configuration.GetSection("mailgun-to").Bind(mailgunTo);
            foreach (string emailTo in mailgunTo)
                request.AddParameter("to", emailTo);
        }

        private static void ConfigureRequestFrom(IConfigurationRoot configuration, RestRequest request)
        {
            string mailgunFrom = configuration.GetSection("mailgun-from").Value;
            request.AddParameter("from", mailgunFrom);
        }

        private static void ConfigureRequestDomain(IConfigurationRoot configuration, RestRequest request)
        {
            string mailgunDomain = configuration.GetSection("mailgun-domain").Value;

            request.AddParameter("domain", mailgunDomain, ParameterType.UrlSegment);
            request.Resource = $"{mailgunDomain}/messages";
        }

        private static void ConfigureClient(IConfigurationRoot configuration, RestClient client)
        {
            string mailgunApiKey = configuration.GetSection("mailgun-apikey").Value;

            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", mailgunApiKey);
        }

        private static List<Question> QueryRandomQuestions()
        {
            return _context.Questions
                           .Where(q => q.DateUsedUTC == null)
                           .OrderByDescending(q => q.PreferredQuestion)
                           .ThenBy(r => Guid.NewGuid())
                           .Take(3)
                           .ToList();
        }

        private static void SeedEmptyDatabase()
        {
            if (_context.Questions.Count() == 0)
            {
                SeedData.SeedQuestions(_context, @"C:\Users\Jonathan\source\repos\QuestionGenerator\QuestionGenerator.Data\ProcessedSeedData\BookOfQuestions.txt");
                SeedData.SeedQuestions(_context, @"C:\Users\Jonathan\source\repos\QuestionGenerator\QuestionGenerator.Data\ProcessedSeedData\QuestionsForCouples.txt");
            }
        }

        private static void InitializeDbContext(IConfigurationRoot configuration)
        {
            string connString = configuration.GetConnectionString("QuestionDatabase");
            _context = new QuestionDbContext(connString);
        }

        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return configuration;
        }
    }
}
