using QuestionGenerator.Data;
using QuestionGenerator.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using RestSharp.Authenticators;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace QuestionGenerator.ConsoleUI
{
    class Program
    {
        private const int NUMBER_OF_QUESTIONS_TO_GENERATE = 3;
        private static IRepository _repository;

        static void Main(string[] args)
        {
            IConfigurationRoot configuration = GetConfiguration();
            InitializeDbContext(configuration);
            SeedEmptyDatabase();
            List<Question> randomQuestions = QueryRandomQuestions(NUMBER_OF_QUESTIONS_TO_GENERATE);
            EmailQuestions(configuration, randomQuestions);
            UpdateQuestionsInDatabase(randomQuestions);
        }

        private static void UpdateQuestionsInDatabase(List<Question> randomQuestions)
        {
            foreach(Question question in randomQuestions)
            {
                question.DateUsedUTC = DateTime.Today;
                _repository.UpdateDateUsed(question);
            }
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

        private static List<Question> QueryRandomQuestions(int numberOfQuestions)
        {
            return _repository.GetRandomQuestions(numberOfQuestions);
        }

        private static void SeedEmptyDatabase()
        {
            if (_repository.QuestionCount() == 0)
            {
                SeedData.SeedQuestions(_repository, @"C:\Users\Jonathan\source\repos\QuestionGenerator\QuestionGenerator.Data\Questions.txt");
            }
        }

        private static void InitializeDbContext(IConfigurationRoot configuration)
        {
            string connString = configuration.GetConnectionString("QuestionDatabase");
            _repository = new SqliteRepository(new QuestionDbContext(connString));
            //TODO: Add the schema migration to the SqliteRepository
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
