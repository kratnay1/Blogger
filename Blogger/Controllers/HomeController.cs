using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogger.Controllers
{
    public class HomeController : Controller
    {
        //If you visit the home page: https://localhost/, we just redirect to the blogs controller
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Blogs");
        }

       
    }
}