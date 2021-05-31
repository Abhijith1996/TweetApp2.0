using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetApp2._0.Model
{
    public class Tweet
    {
        public string LoginID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Message { get; set; }
        public string Tag {get; set; }
        public DateTime TweetDateTime { get; set; }
        public bool Reaction { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
    }
}
