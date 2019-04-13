using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionGenerator.Domain
{
    public class UserQuestion
    {
        public int UserQuestionID { get; set; }
        public int UserID { get; set; }
        public int QuestionID { get; set; }
        public DateTime DateSent { get; set; }

        public User User { get; set; }
        public Question Question { get; set; }
    }
}
