using QuestionGenerator.Domain;
using System;
using System.Collections.Generic;

namespace QuestionGenerator.Data
{
    public interface IRepository
    {
        List<Question> GetRandomQuestions(int numQuestions);
        void AddQuestion(Question question);
        void UpdateDateUsed(Question question);
        int QuestionCount();
        List<Question> SearchQuestions(string searchText);
    }
}
