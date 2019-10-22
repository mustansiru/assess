using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace assess.Models
{
    public class Question
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string QuestionHeading { get; set; }
        public string Content { get; set; }
        public string ContentImageUrl { get; set; }
        public List<Choice> CorrectChoices { get; set; }
        public List<Choice> Choices { get; set; }
        public decimal Score { get; set; }
    }
}
