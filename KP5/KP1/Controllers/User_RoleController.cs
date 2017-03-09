using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KP1.Models;

namespace KP1.Controllers
{
    public class User_RoleController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: User_Role
        public ActionResult Index()
        {
            var user_Role = db.User_Role.Include(u => u.Role).Include(u => u.User);
            return View(user_Role.ToList());
        }

        // GET: User_Role/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Role user_Role = db.User_Role.Find(id);
            if (user_Role == null)
            {
                return HttpNotFound();
            }
            return View(user_Role);
        }

        // GET: User_Role/Create
        public ActionResult Create()
        {
            ViewBag.Id_Role = new SelectList(db.Role, "Id", "Role1");
            ViewBag.Id_User = new SelectList(db.User, "Id", "Login");
            return View();
        }

        // POST: User_Role/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_User,Id_Role")] User_Role user_Role)
        {
            if (ModelState.IsValid)
            {
                db.User_Role.Add(user_Role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Role = new SelectList(db.Role, "Id", "Role1", user_Role.Id_Role);
            ViewBag.Id_User = new SelectList(db.User, "Id", "Login", user_Role.Id_User);
            return View(user_Role);
        }

        // GET: User_Role/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Role user_Role = db.User_Role.Find(id);
            if (user_Role == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Role = new SelectList(db.Role, "Id", "Role1", user_Role.Id_Role);
            ViewBag.Id_User = new SelectList(db.User, "Id", "Login", user_Role.Id_User);
            return View(user_Role);
        }

        // POST: User_Role/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_User,Id_Role")] User_Role user_Role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user_Role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Role = new SelectList(db.Role, "Id", "Role1", user_Role.Id_Role);
            ViewBag.Id_User = new SelectList(db.User, "Id", "Login", user_Role.Id_User);
            return View(user_Role);
        }

        // GET: User_Role/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User_Role user_Role = db.User_Role.Find(id);
            if (user_Role == null)
            {
                return HttpNotFound();
            }
            return View(user_Role);
        }

        // POST: User_Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User_Role user_Role = db.User_Role.Find(id);
            db.User_Role.Remove(user_Role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
