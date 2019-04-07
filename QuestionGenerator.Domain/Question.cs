using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionGenerator.Domain
{
    public class Question
    {
        public int QuestionID { get; set; }
        public bool PreferredQuestion { get; set; }
        public DateTime? DateUsedUTC { get; set; }
        public string QuestionText { get; set; }

        public List<UserQuestion> UserQuestions { get; set; }
    }
}
