using System.Collections.Generic;

namespace QuestionGenerator.Notifications
{
    public interface INotifier
    {
        void SetRecipients(IEnumerable<string> recipients);
        void SetSubject(string subject);
        void SetFrom(string from);
        void SendMessage(string message);
    }
}
