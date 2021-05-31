using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp2._0.Model;
using TweetApp2._0.Services;

namespace TweetApp2._0.Controllers
{
    [ApiController]
    [Route("api/v1.0/tweet")]
    public class UserController : ControllerBase
    {


        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }       

        [HttpPost]
        [Route("Register")]
        public ActionResult <UserInfo> Register([FromBody] UserInfo user)
        {

            user.Password = encryptpass(user.Password);
           var regUser= _userService.Register(user);
            if (regUser.LoginID == null)
                return StatusCode(500, "Login ID alreday taken");
            else
                return regUser;
        }
        [HttpGet]
        [Route("Login")]
        public ActionResult<UserInfo> Login( string loginID, string password)
        {

            var encrptedpass= encryptpass(password);
            var status = _userService.Login(loginID, encrptedpass);
            if (status.LoginID == null)
                return StatusCode(401, "Invalid loginID/Password"+ new UserInfo());
            else
                return status;
        }
        [HttpGet]
        [Route("all")]
        public ActionResult<List<Tweet>> GetAllTweet()
        {
            return _userService.GetAllTweets();
        }
        [HttpPost]
        [Route("{username}/add")]
        public ActionResult<Tweet> PostTweet([FromBody]  Tweet tweet,  string username)
        {
            tweet.LoginID = username;
            tweet.TweetDateTime = DateTime.Now;
            //tweet.TweetDateTime = DateTime.ParseExact(DateTime.Now.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (tweet.Message != null)
                return _userService.PostTweet(tweet);
            else
                return StatusCode(500, "Tweet Message is Empty");
        }

        [HttpGet]
        [Route("{username}")]
        public ActionResult<List<Tweet>> GetTweetByID(string username)
        {
            if (username != null)
                return _userService.GetTweetsByID(username);
            else
                return StatusCode(401, "LoginID Not found");
        }
        [HttpGet]
        [Route("currentLoggedUser")]
        public ActionResult<UserInfo> GetCurrentLoggedinUser()

        {
            return this._userService.GetCurrentLoggedUserID();
        }

       

        [HttpDelete]
        [Route("{username}/delete/{id}")]
        public ActionResult<Boolean> DeleteTweet(string username, string id)
        {
            return _userService.DeleteTweet(username, id);
        }

        [HttpPut]
        [Route("{username}/update/{id}")]
        public ActionResult<Tweet> UpdateTweet(string username, string id, [FromBody] Tweet tweet)
        {
            return _userService.UpdateTweet(username, id, tweet);
        }

        [HttpGet]
        [Route("users/all")]
        public ActionResult<List<UserInfo>> GetAllUsers()
        {
            return _userService.GetAllUsers();
        }

        [HttpGet]
        [Route("{username}/forget")]
        public ActionResult<Boolean> ForgetPassword(string username, string conatct, string  password)
        {

            return _userService.ForgetPassword(username,conatct,encryptpass(password));
        }

        

        [NonAction]
        public string encryptpass(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }
    }
}
