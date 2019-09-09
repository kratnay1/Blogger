using Blogger.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogger.Controllers
{
    // Url: /posts/
    public class PostsController : Controller
    {
        //If you go to, /posts/{id}/, it should display the post and any comments for the post
        public ActionResult Index(int? id)
        {
            if (id.HasValue == false)
            {
                throw new Exception("To view a blog post, you must pass a PostId");
            }
            PostViewModel model = new PostViewModel();
            model.Comments = new List<PostViewModel.Comment>(); //Initialize array so it is not null because we need to add to it below

            string sql = @"
                SELECT PostId, PostTitle, MessageContent, CreatedDate
                FROM BlogPosts
                WHERE PostId = " + id;

            bool postFound = false;
            //Okay, let's first load the post information for this PostId that was passed in (/posts/{id}/)
            using (SqlConnection conn = new SqlConnection(CommonUtility.GetMainConnectionstring()))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        model.PostId = reader.GetInt32(reader.GetOrdinal("PostId"));
                        model.PostTitle = reader.GetString(reader.GetOrdinal("PostTitle"));
                        model.MessageContent = reader.GetString(reader.GetOrdinal("MessageContent"));
                        model.CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
                        postFound = true;
                    }
                }
            }

            //Bogus postId that doesn't exist in the system was passed to /posts/{id}/, so throw error
            if (!postFound)
            {
                throw new Exception("Post with PostId " + id + " was not found in system.");
            }

            //Okay, now that we got the post information loaded into the model, let's load the comments for
            //this post
            sql = @"
                SELECT CommentId, PostId, Username, MessageContent, CreatedDate
                FROM BlogPostComments
                WHERE PostId = " + id + @"
                ORDER BY CreatedDate DESC";
            using (SqlConnection conn = new SqlConnection(CommonUtility.GetMainConnectionstring()))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    SqlDataReader reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        PostViewModel.Comment comment = new PostViewModel.Comment();
                        comment.CommentId = reader.GetInt32(reader.GetOrdinal("CommentId"));
                        comment.PostId = reader.GetInt32(reader.GetOrdinal("PostId"));
                        comment.Username = reader.GetString(reader.GetOrdinal("Username"));
                        comment.MessageContent = reader.GetString(reader.GetOrdinal("MessageContent"));
                        comment.CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"));
                        model.Comments.Add(comment);
                    }
                }
            }


            //Okay, we got all the information, lets return the data structure we loaded back to the view for 
            //html display
            return View(model);
        }


        [HttpPost]
        public ActionResult Index(int? id, string username, string messageContent)
        {
            //Okay, user tries to post a blog comment, let's validate and make sure
            //the input the user entered is not empty
            if (!id.HasValue)
            {
                throw new Exception("To post a comment for a blog post, you must pass in a post id");
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new Exception("You must enter a username to post a comment");
            }

            if (string.IsNullOrWhiteSpace(messageContent))
            {
                throw new Exception("You must enter a message to post a comment");
            }

            //Okay, validations passed, let's connect to the database and insert the comment

            string sql = @"
                INSERT INTO BlogPostComments(PostId, Username, MessageContent)
                VALUES(" + id + ", '" + username + "', '" + messageContent + "')";
            using (SqlConnection conn = new SqlConnection(CommonUtility.GetMainConnectionstring()))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }

            //Okay, blog post comment inserted. Now let's redirect back to the blog post so we can
            //see the comment that we entered. The /posts/{id}/ needs the "post id" so that is why we're
            //doing new { id = id }
            return RedirectToAction("Index", new { id = id });
        }
    }
}