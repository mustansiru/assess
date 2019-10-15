using System;
using System.Collections.Generic;
using assess.Models;
using MongoDB.Driver;

namespace assess.Services
{
    public class QuestionCategoryService
    {
        private readonly IMongoCollection<QuestionCategory> _categories;

        public QuestionCategoryService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _categories = database.GetCollection<QuestionCategory>("QuestionCategory");
        }

        // GET
        public List<QuestionCategory> Get() =>
            _categories.Find(category => true).ToList();

        // GET ONE
        public QuestionCategory Get(string id) =>
            _categories.Find(cat => cat.Id == id).FirstOrDefault();

        // CREATE ONE
        public QuestionCategory Create(QuestionCategory category)
        {
            // insert into DB
            _categories.InsertOne(category);
            return category;
        }

        public void Update(string id, QuestionCategory category) =>
            _categories.ReplaceOne(cat => cat.Id == id, category);

        public void Remove(QuestionCategory category) =>
            _categories.DeleteOne(cat => cat.Id == category.Id);

        public void Remove(string id) =>
            _categories.DeleteOne(cat => cat.Id == id);
    }
}
