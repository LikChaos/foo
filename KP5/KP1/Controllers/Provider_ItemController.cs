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
    public class Provider_ItemController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: Provider_Item
        public ActionResult Index()
        {
            var provider_Item = db.Provider_Item.Include(p => p.Item).Include(p => p.Provider);
            return View(provider_Item.ToList());
        }

        // GET: Provider_Item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider_Item provider_Item = db.Provider_Item.Find(id);
            if (provider_Item == null)
            {
                return HttpNotFound();
            }
            return View(provider_Item);
        }

        // GET: Provider_Item/Create
        public ActionResult Create()
        {
            ViewBag.Id_item = new SelectList(db.Item, "Id", "barcode");
            ViewBag.Id_provider = new SelectList(db.Provider, "Id", "Name");
            return View();
        }

        // POST: Provider_Item/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_item,Id_provider")] Provider_Item provider_Item)
        {
            if (ModelState.IsValid)
            {
                db.Provider_Item.Add(provider_Item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_item = new SelectList(db.Item, "Id", "barcode", provider_Item.Id_item);
            ViewBag.Id_provider = new SelectList(db.Provider, "Id", "Name", provider_Item.Id_provider);
            return View(provider_Item);
        }

        // GET: Provider_Item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider_Item provider_Item = db.Provider_Item.Find(id);
            if (provider_Item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_item = new SelectList(db.Item, "Id", "barcode", provider_Item.Id_item);
            ViewBag.Id_provider = new SelectList(db.Provider, "Id", "Name", provider_Item.Id_provider);
            return View(provider_Item);
        }

        // POST: Provider_Item/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_item,Id_provider")] Provider_Item provider_Item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(provider_Item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_item = new SelectList(db.Item, "Id", "barcode", provider_Item.Id_item);
            ViewBag.Id_provider = new SelectList(db.Provider, "Id", "Name", provider_Item.Id_provider);
            return View(provider_Item);
        }

        // GET: Provider_Item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider_Item provider_Item = db.Provider_Item.Find(id);
            if (provider_Item == null)
            {
                return HttpNotFound();
            }
            return View(provider_Item);
        }

        // POST: Provider_Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Provider_Item provider_Item = db.Provider_Item.Find(id);
            db.Provider_Item.Remove(provider_Item);
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
