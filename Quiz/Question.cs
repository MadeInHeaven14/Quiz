using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    class Question
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId _id;
        public string Question_Name { get; set; }
        public string Answer { get; set; }
    }
}
