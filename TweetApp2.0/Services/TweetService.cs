using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetApp2._0.Model;

namespace TweetApp2._0.Services
{
    public interface ITweetService
    {
        public ReplyTweet LikeTweet(string username, string id);
        public ReplyTweet ReplyTweet(string username, string id, string msg);
        public List<ReplyTweet> GetTweetByUser(string user);
        public List<ReplyTweet> GetAllReplyByTweet(string id);
        public List<Tweet> SearchTweetByUser(string username);


    }
    public class TweetService:ITweetService
    {
        private IMongoCollection<UserInfo> _userInfo;
        private IMongoCollection<Tweet> _tweets;

        private IMongoCollection<ReplyTweet> _replytweets;
        public UserInfo _currentLoggedInUser;
        private object newpass;

        public TweetService(ITweetAppMongoDBSettings settings)
        {
            //settings.ConnectionString = "mongodb://localhost:27017";
            //settings.DatabaseName = "TweetApp";
            //settings.UserCollectionName = "UserInfo";
            //settings.TweetCollectionName = "Tweet";
            //settings.ReplyTweetCollectionName = "ReplyTweet";
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _userInfo = database.GetCollection<UserInfo>(settings.UserCollectionName);
            _tweets = database.GetCollection<Tweet>(settings.TweetCollectionName);
            _replytweets = database.GetCollection<ReplyTweet>(settings.ReplyTweetCollectionName);

        }
        public ReplyTweet LikeTweet(string username, string id)
        {
            var temp = _replytweets.Find(a => a.Tweetid == id && a.LikeUser == username).FirstOrDefault();
            if ( temp== null)
            {
                ReplyTweet replyTweet = new ReplyTweet();
                replyTweet.LikeUser = username;
                replyTweet.Reaction = true;
                replyTweet.Tweetid = id;
                _replytweets.InsertOne(replyTweet);
                return replyTweet;
            }
            else
            {
                var filter = Builders<ReplyTweet>.Filter.Eq(x => x.LikeUser, username)
                            & Builders<ReplyTweet>.Filter.Eq(y => y.Tweetid, id);
                if (_replytweets.Find(f => f.Tweetid == id && f.LikeUser == username).FirstOrDefault().Reaction)
                {
                    var update = Builders<ReplyTweet>.Update.Set(q => q.Reaction, false);
                    _replytweets.UpdateOne(filter, update);
                    return new ReplyTweet();
                }
                else
                {
                    var update = Builders<ReplyTweet>.Update.Set(q => q.Reaction, true);
                    _replytweets.UpdateOne(filter, update);
                    return new ReplyTweet();
                }

                }
        }

        public ReplyTweet ReplyTweet(string username, string id, string msg)
        {
            ReplyTweet replyTweet = new ReplyTweet();
            replyTweet.ReplayUser = username;
            replyTweet.ReplyMessage = msg;
            replyTweet.Tweetid = id;
            _replytweets.InsertOne(replyTweet);
            return replyTweet;

        }

        public List<ReplyTweet> GetAllReplyByTweet(string id)
        {
            return _replytweets.Find(t => t.Tweetid == id).ToList();
        }
        public List<ReplyTweet> GetTweetByUser(string user)
        {
            return _replytweets.Find(r => r.LikeUser == user).ToList();
        }


        public List<Tweet> SearchTweetByUser(string username)
        {
            List<Tweet> tweet = this._tweets.Find(x => x.Firstname.ToLower().Contains(username))
                .ToList();
            
            return tweet;
            
        }
    }
    }
