using assess.Models;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;

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

        public Question Get(string id)
        {
            var result = _questions.Find(q => q.Id == id).FirstOrDefault();
            return result;
        }
            

        public List<Question> Get()
        {
            return _questions.Find(q => true).ToList();
        }

        public Question Create(Question question)
        { 
            //if (contents != null)
            //{
            //    var fileId = UploadFile(DateTime.UtcNow.Ticks.ToString(), contents);
            //    question.ContentImageId = fileId;
            //}
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

        public string UploadFile(string fileId, byte[] fileStream)
        {
            var bucket = new GridFSBucket(_questions.Database, new GridFSBucketOptions
            {
                BucketName = "richmedia",
                ChunkSizeBytes = 1048576, // 1MB
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Secondary
            });

            var id = bucket.UploadFromBytes(fileId, fileStream);

            return id.ToString();
        }
    }
}
