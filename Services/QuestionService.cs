using assess.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace assess.Services
{
    public class QuestionService
    {
        private readonly IMongoCollection<Question> _questions;

        public QuestionService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _questions = database.GetCollection<Question>("Questions");
        }

        public List<Question> GetByCategory(string category) =>
            _questions.Find(q => q.Category == category).ToList();

        public List<Question> GetByType(string type) =>
            _questions.Find(q => q.Type == type).ToList();

        public Question Get(string id) =>
            _questions.Find(q => q.Id == id).FirstOrDefault();

        public List<Question> Get()
        {
            return _questions.Find(q => true).ToList();
        }

        public Question Create(Question question)
        {
            // insert into DB
            _questions.InsertOne(question);
            return question;
        }

        public void Update(string id, Question question) =>
           _questions.ReplaceOne(q => q.Id == id, question);

        public void Remove(Question question) =>
            _questions.DeleteOne(q => q.Id == question.Id);

        public void Remove(string id) =>
            _questions.DeleteOne(q => q.Id == id);
    }
}
