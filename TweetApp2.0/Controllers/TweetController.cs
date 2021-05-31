using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetApp2._0.Model;
using TweetApp2._0.Services;

namespace TweetApp2._0.Controllers
{
    [ApiController]
    [Route("api/v1.0/tweet")]
    public class TweetController : Controller
    {
        private readonly ITweetService _tweetService;

        public TweetController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }
        [HttpPut]
        [Route("{username}/like/{id}")]
        public ActionResult<ReplyTweet> LikeTweet(string username, string id)
        {
            return _tweetService.LikeTweet(username, id);
        }

        [HttpPost]
        [Route("{username}/reply/{id}")]
        public ActionResult<ReplyTweet> ReplyTweet(string username, string id,[FromBody]ReplyTweet Message)
        {
            return _tweetService.ReplyTweet(username, id, Message.ReplyMessage);
        }
        [HttpGet]
        [Route("getReplyByTweet/{id}")]
        public ActionResult<List<ReplyTweet>> GetReplyByTweet(string id)
        {
            return _tweetService.GetAllReplyByTweet(id);
        }
        [HttpGet]
        [Route("getLikeForUser/{username}")]
            public ActionResult<List<ReplyTweet>> GetLikeByUser(string username)
        {
            return _tweetService.GetTweetByUser(username);

        }

        [HttpGet]
        [Route("user/search/{username}")]
        public ActionResult<List<Tweet>> SearchByUser(string username)
        {
            return _tweetService.SearchTweetByUser(username);

        }

    }
}
