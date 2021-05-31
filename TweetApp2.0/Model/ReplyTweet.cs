using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetApp2._0.Model
{
    public class ReplyTweet
    {
        public string Tweetid { get; set; }
        public string ReplyMessage { get; set; }
        public string LikeUser { get; set; }
        public string ReplayUser { get; set; }
        public bool Reaction { get; set; }


        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

    }
}
