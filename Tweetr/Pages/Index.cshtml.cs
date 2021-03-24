using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Tweetr.Interfaces;
using Tweetr.Models;

namespace Tweetr.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private ITweet _tweetHandler;
        
        
        public Customer customer { get; set; }
        public bool NotLoggedIn { get; set; }
        [BindProperty]
        public List<Tweet> TweetsPublic { get; set; }
        [BindProperty]
        public List<Tweet> TweetsPrivate { get; set; }
        [BindProperty]
        public Tweet Tweet { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ITweet iTweet)
        {
            _logger = logger;
            _tweetHandler = iTweet;
        }

        public void OnGet()
        {
            TweetsPrivate = new List<Tweet>();
            TweetsPublic = new List<Tweet>();
            if (HttpContext.Session.GetString("user") != null)
            {
                customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
                foreach (Tweet tweets in _tweetHandler.GetAllFriendsTweets(customer.Id).Values)
                {

                    TweetsPrivate.Add(tweets);
                }

            }
            else
            {
                NotLoggedIn = true;
            }
            foreach (Tweet tweets in _tweetHandler.GetAllPublicTweets().Values)
            {

            TweetsPublic.Add(tweets);
            }
        }

        public IActionResult OnPost()
        {
            if (Tweet.Text != null)
            {
                Tweet.customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
                Tweet.DateOfTweet = DateTime.Now;
                _tweetHandler.Create(Tweet);
                return RedirectToPage();
            }
            return RedirectToPage();
        }

        public IActionResult OnPostLikes(int id)
        {
            Tweet tweet = _tweetHandler.GetTweet(id);

            customer = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(HttpContext.Session.GetString("user"));
            _tweetHandler.AddLike(customer.Id,id);
            return RedirectToPage();
        }
        public IActionResult OnPostDelete(int id)
        {
            _tweetHandler.DeleteTweet(id);
            return RedirectToPage();
        }
    }
}
