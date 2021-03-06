﻿using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionGenerator.Notifications
{
    public class MailgunNotifier : INotifier
    {
        public string From { get; set; }
        public List<string> Recipients { get; set; }
        public string Domain { get; set; }
        public string ApiKey { get; set; }

        public MailgunNotifier(MailgunNotifierConfiguration config)
        {
            if(config == null || config.MailgunApiKey == null || config.MailgunDomain == null)
            {
                throw new ArgumentNullException();
            }

            Domain = config.MailgunDomain;
            ApiKey = config.MailgunApiKey;
        }

        public void SendMessage(string subject, string message)
        {
            RestClient client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api", ApiKey)
            };

            RestRequest request = new RestRequest();
            request.AddParameter("domain", Domain, ParameterType.UrlSegment);
            request.Resource = $"{Domain}/messages";
            foreach (string emailTo in Recipients)
                request.AddParameter("to", emailTo);
            request.AddParameter("from", From);
            //request.AddParameter("subject", $"Email Questions for {DateTime.Today.ToString("MM/dd/yyyy")}");
            request.AddParameter("subject", subject);
            request.AddParameter("text", message);
            request.Method = Method.POST;
            
            client.Execute(request);
        }

        public void SetFrom(string from)
        {
            From = from;
        }

        public void SetRecipients(IEnumerable<string> recipients)
        {
            Recipients = recipients.ToList();
        }
    }
}
