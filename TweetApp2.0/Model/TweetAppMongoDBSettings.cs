using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetApp2._0.Model
{
    
        public class TweetAppMongoDBSettings : ITweetAppMongoDBSettings
        {
            public string UserCollectionName { get; set; }
        public string TweetCollectionName { get; set; }

        public string ReplyTweetCollectionName { get; set; }

        public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
        }

        public interface ITweetAppMongoDBSettings
    {
            public string UserCollectionName { get; set; }
        public string TweetCollectionName { get; set; }
        public string ReplyTweetCollectionName { get; set; }

        public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
        }
    }

