using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionGenerator.Domain
{
    public class User
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public List<UserQuestion> UserQuestions { get; set; }
    }
}
