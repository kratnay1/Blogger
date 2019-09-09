using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogger.Models
{
    public class BlogsListingViewModel
    {
        public List<BlogItem> Blogs { get; set; }

        public class BlogItem
        {
            public int BlogId { get; set; }

            public string BlogTitle { get; set; }

            public DateTime CreatedDate { get; set; }

            public int PostCount { get; set; }
        }
    }
}