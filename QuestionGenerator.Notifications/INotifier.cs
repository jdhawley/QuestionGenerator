using System.Collections.Generic;

namespace QuestionGenerator.Notifications
{
    public interface INotifier
    {
        void SetRecipients(IEnumerable<string> recipients);
        void SetFrom(string from);
        void SendMessage(string subject, string message);
    }
}
