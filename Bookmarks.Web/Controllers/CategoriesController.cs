using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookmarks.Models;
using Bookmarks.Data;
using Bookmarks.Web.ViewModels;
using Bookmarks.Web.Filters;
using WebMatrix.WebData;

namespace Bookmarks.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private BookmarksContext db = new BookmarksContext();

        //
        // GET: /Categories/

        [Authorize]
        public ActionResult Index()
        {
            var user = GetUser();
            var categories = db.Categories.Where(x => x.User.UserId == user.UserId && !x.IsDeleted);
            return View(categories.ToList());
        }

        //
        // GET: /Categories/Details/5

        public ActionResult Details(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var categoryViewModel = new CategoriesDetailsViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                IsDeleted = category.IsDeleted,
                NumberOfBookmarks = category.Bookmarks.Count
            };

            return View(categoryViewModel);
        }

        //
        // GET: /Categories/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Categories/Create

        [HttpPost]
        [ValidateAntiForgeryToken, InitializeSimpleMembership]
        public ActionResult Create(Category category)
        {
            var existing = db.Categories.Where(x=>x.Id == category.Id).FirstOrDefault();

            if (existing != null)
            {
                existing.IsDeleted = false;
                db.SaveChanges();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var user = GetUser();
                    category.IsDeleted = false;
                    category.User = user;
                    db.Categories.Add(category);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Categories/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /Categories/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /Categories/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        //
        // POST: /Categories/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken, InitializeSimpleMembership]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            var user = GetUser();
            if (category.User.UserId == user.UserId)
            {
                category.IsDeleted = true;
                db.SaveChanges();   
            }

            return RedirectToAction("Index");
        }

        [Authorize, InitializeSimpleMembership]
        public ActionResult DisplayCategory(string categoryName)
        {
            var user = GetUser();
            var category = db.Categories.
                Where(x => x.Name == categoryName && x.User.UserId == user.UserId && !x.IsDeleted)
                .FirstOrDefault();

            if (category == null)
            {
                return Redirect("Categories/index/");
            }

            ViewBag.CategoryName = db.Categories.Where(x => x.Id == category.Id)
                .Select(x => x.Name)
                .FirstOrDefault();
            var bookmarks = db.Bookmarks
                .Where(x => x.Category.Id == category.Id)
                .Select(x => new DisplayBookmarkViewModel()
                {
                    Id = x.Id,
                    Url = x.Url,
                    Title = x.Title,
                    Description = x.Description,
                    Category = x.Category.Name, 
                    LastEdit = x.LastEdit,
                });

            return View(bookmarks);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        [InitializeSimpleMembership]
        private UserProfile GetUser()
        {
            UserProfile user = db.UserProfiles.Where(x => x.UserId == WebSecurity.CurrentUserId).FirstOrDefault();
            return user;
        }
    }
}