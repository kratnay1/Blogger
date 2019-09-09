using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogger.Models
{
    public class PostListingViewModel
    {
        public string BlogTitle { get; set; }

        public List<PostItem> Posts { get; set; }

        public class PostItem
        {
            public int PostId { get; set; }

            public string PostTitle { get; set; }

            public string MessageContent { get; set; }

            public DateTime CreatedDate { get; set; }

            public int CommentCount { get; set; }
        }
    }

  
}