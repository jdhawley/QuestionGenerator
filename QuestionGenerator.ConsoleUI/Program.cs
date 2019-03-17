using Microsoft.Extensions.Configuration;
using QuestionGenerator.Data;
using QuestionGenerator.Domain;
using QuestionGenerator.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuestionGenerator.ConsoleUI
{
    class Program
    {
        private const int NUMBER_OF_QUESTIONS_TO_GENERATE = 3;
        private static IRepository _repository;
        private static INotifier _notifier;

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
            MailgunNotifierConfiguration mgConfig = new MailgunNotifierConfiguration()
            {
                MailgunDomain = configuration.GetSection("mailgun-domain").Value,
                MailgunApiKey = configuration.GetSection("mailgun-apikey").Value
            };
            _notifier = new MailgunNotifier(mgConfig);

            _notifier.SetFrom(configuration.GetSection("mailgun-from").Value);
            List<string> mailgunTo = new List<string>();
            configuration.GetSection("mailgun-to").Bind(mailgunTo);
            _notifier.SetRecipients(mailgunTo);
            _notifier.SendMessage(
                $"Email Questions for {DateTime.Today.ToString("MM/dd/yyyy")}", 
                String.Join("\n\n", randomQuestions.Select(x => x.QuestionText)));
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
