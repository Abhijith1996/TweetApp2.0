using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TweetApp2._0.Model
{
    public class UserInfo
    {
            
        
        public string EmailID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ContactNo { get; set; }
        public string LoginID { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

    }
}

 
 