using Bookmarks.Data;
using Bookmarks.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Bookmarks.Web.Controllers
{
    public class HomeController : Controller
    {
        private BookmarksContext db = new BookmarksContext();

        [InitializeSimpleMembership]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to Bookmarks.";
            var user = db.UserProfiles.Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
