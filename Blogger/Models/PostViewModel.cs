using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogger.Models
{
    public class PostViewModel
    {
        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public string MessageContent { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<Comment> Comments { get; set; }

        public class Comment
        {
            public int CommentId { get; set; }

            public int PostId { get; set; }

            public string Username { get; set; }

            public string MessageContent { get; set; }

            public DateTime CreatedDate { get; set; }
        }
    }

    
}