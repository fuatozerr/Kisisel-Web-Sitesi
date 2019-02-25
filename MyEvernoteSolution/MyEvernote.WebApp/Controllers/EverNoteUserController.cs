using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;

namespace MyEvernote.WebApp.Controllers
{
    public class EverNoteUserController : Controller
    {
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();

        // GET: EverNoteUser
        public ActionResult Index()
        {
            return View(evernoteUserManager.List());
        }

        // GET: EverNoteUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EverNoteUser everNoteUser = evernoteUserManager.Find(x=>x.Id == id.Value);
            if (everNoteUser == null)
            {
                return HttpNotFound();
            }
            return View(everNoteUser);
        }

        // GET: EverNoteUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EverNoteUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( EverNoteUser everNoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");

            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {

             BusinessLayerResult<EverNoteUser> res=   evernoteUserManager.Insert(everNoteUser);

                if(res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(everNoteUser);
                }
        
                return RedirectToAction("Index");
            }

            return View(everNoteUser);
        }

        // GET: EverNoteUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EverNoteUser everNoteUser = evernoteUserManager.Find(x => x.Id == id.Value);
            evernoteUserManager.Update(everNoteUser);
            if (everNoteUser == null)
            {
                return HttpNotFound();
            }
            return View(everNoteUser);
        }

        // POST: EverNoteUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( EverNoteUser everNoteUser)
        {
            if (ModelState.IsValid)
            {
                evernoteUserManager.Update(everNoteUser);
                return RedirectToAction("Index");
            }
            return View(everNoteUser);
        }

        // GET: EverNoteUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EverNoteUser everNoteUser = evernoteUserManager.Find(x=>x.Id==id.Value);
            if (everNoteUser == null)
            {
                return HttpNotFound();
            }
            return View(everNoteUser);
        }

        // POST: EverNoteUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EverNoteUser everNoteUser = evernoteUserManager.Find(x => x.Id == id);
            evernoteUserManager.Delete(everNoteUser);
            
            return RedirectToAction("Index");
        }

       
    }
}
