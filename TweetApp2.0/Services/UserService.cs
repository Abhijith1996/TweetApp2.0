using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using TweetApp2._0.Model;

namespace TweetApp2._0.Services
{
    public interface IUserService
    {
       // public void RegistrationService(ITweetAppMongoDBSettings settings);
        public List<UserInfo> Get();
        public UserInfo Register(UserInfo user);
        public UserInfo Login(string loginID, string pass);
        public List<Tweet> GetTweetsByID(string loginID);
        public Tweet UpdateTweet(string username, string id, Tweet _tweet);

        public Boolean ForgetPassword(string usrname, string contact, string newpass);

        public List<Tweet> GetAllTweets();
        public List<UserInfo> GetAllUsers();
        public Tweet PostTweet(Tweet tweet);
        public UserInfo GetCurrentLoggedUserID();
        

        public Boolean DeleteTweet(string username, string id);

    }
    public class UserService:IUserService
    {
        private IMongoCollection<UserInfo> _userInfo;
        private IMongoCollection<Tweet> _tweets;
        public UserInfo _currentLoggedInUser;


        public UserService(ITweetAppMongoDBSettings settings)
        {
            //settings.ConnectionString = "mongodb://localhost:27017";
            //settings.DatabaseName = "TweetApp";
            //settings.UserCollectionName = "UserInfo";
            //settings.TweetCollectionName = "Tweet";
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _userInfo = database.GetCollection<UserInfo>(settings.UserCollectionName);
            _tweets = database.GetCollection<Tweet>(settings.TweetCollectionName);

        }
        public List<UserInfo> Get()
        {
            List<UserInfo> users;
            users = _userInfo.Find(e=>true).ToList();
            return users;
        }
        public UserInfo GetCurrentLoggedUserID()
        {
            return _currentLoggedInUser;
        }

        public UserInfo Register(UserInfo newUser)
        {
            
             var login = _userInfo.Find(e => e.LoginID == newUser.LoginID).FirstOrDefault();
            if (login == null)
            {
                _userInfo.InsertOne(newUser);
                _currentLoggedInUser = newUser;
                return newUser;
            }
            else
            {
                _currentLoggedInUser = login;
                return new UserInfo();
            }
            
        }

        public UserInfo Login(string loginID, string pass)
        {

            var login = _userInfo.Find(e => e.LoginID == loginID).FirstOrDefault();
            if (login != null)
            {
                if (login.Password == pass)
                {
                    _currentLoggedInUser = login;
                    return login;
                }

                return new UserInfo();

            }
            else
                return new UserInfo ();

        }
         
        public List<Tweet> GetAllTweets()
        {
            List<Tweet> tweets = _tweets.Find(e =>true).ToList();
            return tweets;

        }

        public Tweet PostTweet(Tweet tweet)
        {
            tweet.Firstname = _currentLoggedInUser.FirstName;
            tweet.Lastname = _currentLoggedInUser.LastName;

            _tweets.InsertOne(tweet);
            return tweet;
        }
         
        public List<Tweet> GetTweetsByID(string loginID)
        {
             return _tweets.Find(t => t.LoginID == loginID).ToList();
        }

       

        public Boolean DeleteTweet(string username, string id)
        {
            _tweets.DeleteOne(f => f.LoginID == username && f.id == id);
            return true;
        }

        public Tweet UpdateTweet(string username, string id, Tweet _tweet)
        {

            DeleteTweet(username, id);
            PostTweet(_tweet);
            return _tweet;            
   
        }

        public List<UserInfo> GetAllUsers() {

            return _userInfo.Find(t => true).ToList();
        }

        public Boolean ForgetPassword(string usrname, string contact, string newpass)
        {


            
             if (_userInfo.Find(f => f.LoginID == usrname && f.ContactNo == contact).FirstOrDefault()!=null)
            {
                var filter = Builders<UserInfo>.Filter.Eq(x => x.LoginID, usrname)
                                             & Builders<UserInfo>.Filter.Eq(y => y.ContactNo, contact);
                var update = Builders<UserInfo>.Update.Set(q => q.Password, newpass);
                _userInfo.UpdateOne(filter, update);
                return true;
            }
             else
            return false;
        }


    }
}
