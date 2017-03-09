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
    public class ShopQuestsController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: ShopQuests
        public ActionResult Index()
        {
            var shopQuest = db.ShopQuest.Include(s => s.Provider);
            return View(shopQuest.ToList());
        }

        // GET: ShopQuests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopQuest shopQuest = db.ShopQuest.Find(id);
            if (shopQuest == null)
            {
                return HttpNotFound();
            }
            return View(shopQuest);
        }

        // GET: ShopQuests/Create
        public ActionResult Create()
        {
            ViewBag.Id_Provider = new SelectList(db.Provider, "Id", "Name");
            return View();
        }

        // POST: ShopQuests/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Id_Provider,State")] ShopQuest shopQuest)
        {
            if (ModelState.IsValid)
            {
                db.ShopQuest.Add(shopQuest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Provider = new SelectList(db.Provider, "Id", "Name", shopQuest.Id_Provider);
            return View(shopQuest);
        }

        // GET: ShopQuests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopQuest shopQuest = db.ShopQuest.Find(id);
            if (shopQuest == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Provider = new SelectList(db.Provider, "Id", "Name", shopQuest.Id_Provider);
            return View(shopQuest);
        }

        // POST: ShopQuests/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Id_Provider,State")] ShopQuest shopQuest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shopQuest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Provider = new SelectList(db.Provider, "Id", "Name", shopQuest.Id_Provider);
            return View(shopQuest);
        }

        // GET: ShopQuests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopQuest shopQuest = db.ShopQuest.Find(id);
            if (shopQuest == null)
            {
                return HttpNotFound();
            }
            return View(shopQuest);
        }

        // POST: ShopQuests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShopQuest shopQuest = db.ShopQuest.Find(id);
            db.ShopQuest.Remove(shopQuest);
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
