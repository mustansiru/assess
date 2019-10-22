using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace assess.Models
{
    public class Choice
    {
        public int Order { get; set; }
        public string ChoiceText { get; set; }
        public string ChoiceImage { get; set; }
    }
}
