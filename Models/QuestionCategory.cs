using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace assess.Models
{
    public class QuestionCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        [JsonProperty("Name")]
        public string CategoryName { get; set; }
    }
}
