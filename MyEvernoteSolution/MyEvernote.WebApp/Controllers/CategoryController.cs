 using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.BusinessLayer;
using MyEvernote.Entities;

namespace MyEvernote.WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryManager CategoryManager = new CategoryManager();
        // GET: Category
        public ActionResult Index()
        {
           /* if(Session["login"]==null)
            {
                return RedirectToAction("Index", "Home");
            }*/
            return View(CategoryManager.List());
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = CategoryManager.Find(x=>x.Id==id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");

            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                CategoryManager.Insert(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = CategoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");

            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Category cat = CategoryManager.Find(x => x.Id == category.Id);
                cat.Title = category.Title;
                cat.Description = category.Description;


                CategoryManager.Update(cat);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = CategoryManager.Find(x=>x.Id==id.Value);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = CategoryManager.Find(x => x.Id == id);
            CategoryManager.Delete(category);

            return RedirectToAction("Index");
        }

       
    }
}
