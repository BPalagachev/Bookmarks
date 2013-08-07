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
    public class BookmarksController : Controller
    {
        private BookmarksContext db = new BookmarksContext();

        //
        // GET: /Bookmarks/

        [HttpGet, Authorize, InitializeSimpleMembership]
        public ActionResult Index(string categoryName, string title, string url)
        {
            var user = GetUser();
            var userCategories = db.Categories
                .Where(x => x.User.UserId == user.UserId && !x.IsDeleted)
                .Select(x => x.Name)
                .ToList();

            ViewData["Category"] = new SelectList(userCategories);

            var bookmarks = db.Bookmarks
                .Where(x => x.User.UserId == user.UserId && !x.IsDeleted);

            if (!string.IsNullOrEmpty(categoryName))
            {
                bookmarks = bookmarks.Where(x => x.Category.Name.Contains(categoryName));
            }
            if (!string.IsNullOrEmpty(title))
            {
                bookmarks = bookmarks.Where(x => x.Title.Contains(title));
            }
            if (!string.IsNullOrEmpty(url))
            {
                bookmarks = bookmarks.Where(x => x.Url.Contains(url));
            }

            var model = bookmarks
                .Select(x => new DisplayBookmarkViewModel()
                 {
                     Id = x.Id,
                     Url = x.Url,
                     Title = x.Title,
                     Description = x.Description,
                     Category = x.Category.Name,
                     LastEdit = x.LastEdit,
                 });

            return View(model);
        }

        //
        // GET: /Bookmarks/Details/5

        public ActionResult Details(int id = 0)
        {
            Bookmark bookmark = db.Bookmarks
                .Include("User")
                .Include("Category")
                .Where(x => x.Id == id).FirstOrDefault();
            if (bookmark == null)
            {
                return HttpNotFound();
            }

            var model = new DisplayBookmarkViewModel(bookmark);

            return View(model);
        }

        //
        // GET: /Bookmarks/Create

        [Authorize, InitializeSimpleMembership]
        public ActionResult Create()
        {
            var user = GetUser();
            var userCategories = db.Categories
                .Where(x => x.User.UserId == user.UserId && !x.IsDeleted)
                .Select(x => x.Name)
                .ToList();

            ViewData["Category"] = new SelectList(userCategories);
            return View();
        }

        //
        // POST: /Bookmarks/Create

        [HttpPost]
        [ValidateAntiForgeryToken, InitializeSimpleMembership]
        public ActionResult Create(ManageBookmarkDataViewModel bookmarkModel)
        {
            if (string.IsNullOrEmpty(bookmarkModel.Title))
            {
                bookmarkModel.Title = bookmarkModel.Url;
            }
            var user = GetUser();
            var existing = db.Bookmarks.Where(x => x.Url == bookmarkModel.Url &&x.User.UserId == user.UserId).FirstOrDefault();
            if (existing != null)
            {
                if (ModelState.IsValid && existing.User.UserId == user.UserId)
                {
                    existing.IsDeleted = false;
                    existing.LastEdit = DateTime.Now;
                    existing.Title = bookmarkModel.Title;
                    existing.Url = bookmarkModel.Url;
                    existing.Description = bookmarkModel.Description;
                    existing.Category = LoadOrCreateCategory(bookmarkModel.Category);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var newBookmark = new Bookmark();
                    newBookmark.IsDeleted = false;
                    newBookmark.LastEdit = DateTime.Now;
                    newBookmark.Title = bookmarkModel.Title;
                    newBookmark.Url = bookmarkModel.Url;
                    newBookmark.Description = bookmarkModel.Description;
                    newBookmark.User = user;
                    newBookmark.Category = LoadOrCreateCategory(bookmarkModel.Category);

                    db.Bookmarks.Add(newBookmark);
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Bookmarks/Edit/5

        [InitializeSimpleMembership]
        public ActionResult Edit(int id = 0)
        {
            Bookmark bookmark = db.Bookmarks.Find(id);
            if (bookmark == null)
            {
                return HttpNotFound();
            }

            var user = GetUser();
            var userCategories = db.Categories
                .Where(x => x.User.UserId == user.UserId && !x.IsDeleted)
                .Select(x => x.Name)
                .ToList();

            ViewData["Category"] = new SelectList(userCategories);

            ManageBookmarkDataViewModel model = new ManageBookmarkDataViewModel()
            {
                Id = bookmark.Id,
                Description = bookmark.Description,
                Title = bookmark.Title,
                Url = bookmark.Url,
                Category = bookmark.Category.Name
            };

            return View(model);
        }

        //
        // POST: /Bookmarks/Edit/5

        [HttpPost]
        [InitializeSimpleMembership]
        [ValidateAntiForgeryToken, Authorize]
        public ActionResult Edit(ManageBookmarkDataViewModel bookmarkModel)
        {
            if (string.IsNullOrEmpty(bookmarkModel.Title))
            {
                bookmarkModel.Title = bookmarkModel.Url;
            }

            var user = GetUser();
            var existing = db.Bookmarks.Where(x => x.Url == bookmarkModel.Url).FirstOrDefault();
            if (existing != null)
            {
                if (ModelState.IsValid && existing.User.UserId == user.UserId)
                {
                    existing.IsDeleted = false;
                    existing.LastEdit = DateTime.Now;
                    existing.Title = bookmarkModel.Title;
                    existing.Url = bookmarkModel.Url;
                    existing.Category = LoadOrCreateCategory(bookmarkModel.Category);
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Bookmarks/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Bookmark bookmark = db.Bookmarks.Find(id);
            if (bookmark == null)
            {
                return HttpNotFound();
            }

            return View(bookmark);
        }

        //
        // POST: /Bookmarks/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken, InitializeSimpleMembership]
        public ActionResult DeleteConfirmed(int id)
        {
            Bookmark bookmark = db.Bookmarks.Find(id);
            var user = GetUser();
            if (bookmark.User.UserId == user.UserId)
            {
                bookmark.IsDeleted = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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

        [InitializeSimpleMembership]
        private Category LoadOrCreateCategory(string categoryName)
        {
            var user = GetUser();
            var existing = db.Categories.Where(x => x.Name == categoryName).FirstOrDefault();
            if (existing == null)
            {
                var newCategory = new Category()
                {
                    IsDeleted = false,
                    Name = categoryName,
                    User = user
                };
                db.SaveChanges();

                return newCategory;
            }
            else
            {
                return existing;
            }
        }
    }
}