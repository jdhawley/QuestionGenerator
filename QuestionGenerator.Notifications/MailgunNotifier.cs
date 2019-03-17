using System;
using System.Collections.Generic;

namespace QuestionGenerator.Notifications
{
    public class MailgunNotifier : INotifier
    {
        public string From { get; set; }
        public string Recipients { get; set; }
        public string Subject { get; set; }
        public string Domain { get; set; }
        public string ApiKey { get; set; }

        public MailgunNotifier(MailgunNotifierConfiguration config)
        {
            if(config == null || config.ApiKey == null || config.MailgunDomain == null)
            {
                throw new ArgumentNullException();
            }

            Domain = config.MailgunDomain;
            ApiKey = config.ApiKey;
        }

        public void SendMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void SetFrom(string from)
        {
            throw new NotImplementedException();
        }

        public void SetRecipients(IEnumerable<string> recipients)
        {
            throw new NotImplementedException();
        }

        public void SetSubject(string subject)
        {
            throw new NotImplementedException();
        }
    }
}
