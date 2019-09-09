using Blogger.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogger.Controllers
{
    // Url: /blogs/
    public class BlogsController : Controller
    {
        //If you go to, /blogs/, it should display all of the Blogs (My Personal Blog, My Programming Blog)
        //in the system
        public ActionResult Index()
        {
            BlogsListingViewModel model = new BlogsListingViewModel();
            model.Blogs = new List<BlogsListingViewModel.BlogItem>(); //Initialize array so it is not null because we need to add to it below

            string sql = @"
                SELECT A.BlogId, A.BlogTitle, A.CreatedDate ,
                        (SELECT COUNT(*) FROM BlogPosts B WHERE B.BlogId = A.BlogId) AS PostCount
                FROM Blogs A
            ";
            using (SqlConnection conn = new SqlConnection(CommonUtility.GetMainConnectionstring()))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    while(reader.Read())
                    {
                        BlogsListingViewModel.BlogItem blog = new BlogsListingViewModel.BlogItem();
                        blog.BlogId = reader.GetInt32(reader.GetOrdinal("BlogId"));
                        blog.BlogTitle = reader.GetString(reader.GetOrdinal("BlogTitle"));
                        blog.CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
                        blog.PostCount = reader.GetInt32(reader.GetOrdinal("PostCount"));
                        model.Blogs.Add(blog);
                    }
                }
            }

            //Okay, we got all the information, lets return the data structure we loaded back to the view for 
            //html display
            return View(model);
        }

        //If you go to, /blogs/view/{blogId}/, it should display all of the posts in the blog
        //For example, if you go to /blogs/view/1/, it should display all of the posts for BlogId 1
        public ActionResult View(int? id)
        {
            if(id.HasValue == false)
            {
                throw new Exception("To view a blog, you must pass a BlogId");
            }

            PostListingViewModel model = new PostListingViewModel();
            model.Posts = new List<PostListingViewModel.PostItem>(); //Initialize array so it is not null because we need to add to it below

            bool blogFound = false;
            //First, let's load the blog using the "blogId" that was passed in
            string sql = "SELECT BlogId, BlogTitle FROM Blogs WHERE BlogId = " + id;
            using (SqlConnection conn = new SqlConnection(CommonUtility.GetMainConnectionstring()))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        model.BlogTitle = reader.GetString(reader.GetOrdinal("BlogTitle"));
                        blogFound = true;
                    }
                }
            }

            //Bogus blogId that doesn't exist in the system was passed to /blogs/view/{id}/, so throw error
            if (!blogFound)
            {
                throw new Exception("Blog with BlogId " + id + " was not found in system.");
            }

            //Okay, now we loaded the blog information (blog title etc) for blogId
            //Let's now load the posts for this blog
            sql = @"
                SELECT A.PostId, A.PostTitle, A.MessageContent, A.CreatedDate,
                        (SELECT COUNT(*) FROM BlogPostComments B WHERE B.PostId = A.PostId) AS CommentCount
                FROM BlogPosts A
                WHERE A.BlogId = " + id + @"
                ORDER BY A.CreatedDate DESC";

            using (SqlConnection conn = new SqlConnection(CommonUtility.GetMainConnectionstring()))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        PostListingViewModel.PostItem post = new PostListingViewModel.PostItem();
                        post.PostId = reader.GetInt32(reader.GetOrdinal("PostId"));
                        post.PostTitle = reader.GetString(reader.GetOrdinal("PostTitle"));
                        post.MessageContent = reader.GetString(reader.GetOrdinal("MessageContent"));
                        post.CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
                        post.CommentCount = reader.GetInt32(reader.GetOrdinal("CommentCount"));
                        model.Posts.Add(post);
                    }
                }
            }

            //Okay, we got all the information, lets return the data structure we loaded back to the view for 
            //html display
            return View(model);
        }

        [HttpPost]
        public ActionResult View(int? id, string postTitle, string messageContent)
        {
            //Okay, user tries to post a blog post, let's validate and make sure
            //the input the user entered is not empty
            if (!id.HasValue)
            {
                throw new Exception("To a blog post, you must pass in a blog id");
            }

            if (string.IsNullOrWhiteSpace(postTitle))
            {
                throw new Exception("You must enter a Post Title");
            }

            if (string.IsNullOrWhiteSpace(messageContent))
            {
                throw new Exception("You must enter a message");
            }

            //Okay, validations passed, let's connect to the database and insert the comment

            string sql = @"
                INSERT INTO BlogPosts(BlogId, PostTitle, MessageContent)
                VALUES(" + id + ", '" + postTitle + "', '" + messageContent + "')";
            using (SqlConnection conn = new SqlConnection(CommonUtility.GetMainConnectionstring()))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }

            //Okay, blog post inserted. Now let's redirect back to the blog so we can
            //see the post that we entered. The /blogs/view/{id}/ needs the "blog id" so that is why we're
            //doing new { id = id }
            return RedirectToAction("View", new { id = id });
        }
    }
}