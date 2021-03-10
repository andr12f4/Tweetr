using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetr.Models
{
    public class Tweet
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<Customer> Likes { get; set; }
          public int UserId { get; set; }

    }
}
