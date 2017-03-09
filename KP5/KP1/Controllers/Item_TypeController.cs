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
    public class Item_TypeController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: Item_Type
        public ActionResult Index()
        {
            var item_Type = db.Item_Type.Include(i => i.Item).Include(i => i.ItemType);
            return View(item_Type.ToList());
        }

        // GET: Item_Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item_Type item_Type = db.Item_Type.Find(id);
            if (item_Type == null)
            {
                return HttpNotFound();
            }
            return View(item_Type);
        }

        // GET: Item_Type/Create
        public ActionResult Create()
        {
            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode");
            ViewBag.Id_ItemType = new SelectList(db.ItemType, "Id", "Name");
            return View();
        }

        // POST: Item_Type/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Item,Id_ItemType")] Item_Type item_Type)
        {
            if (ModelState.IsValid)
            {
                db.Item_Type.Add(item_Type);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode", item_Type.Id_Item);
            ViewBag.Id_ItemType = new SelectList(db.ItemType, "Id", "Name", item_Type.Id_ItemType);
            return View(item_Type);
        }

        // GET: Item_Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item_Type item_Type = db.Item_Type.Find(id);
            if (item_Type == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Item = new SelectList(db.Item, "Id", "info", item_Type.Id_Item);
            ViewBag.Id_ItemType = new SelectList(db.ItemType, "Id", "Name", item_Type.Id_ItemType);
            return View(item_Type);
        }

        // POST: Item_Type/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Item,Id_ItemType")] Item_Type item_Type)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item_Type).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode", item_Type.Id_Item);
            ViewBag.Id_ItemType = new SelectList(db.ItemType, "Id", "Name", item_Type.Id_ItemType);
            return View(item_Type);
        }

        // GET: Item_Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item_Type item_Type = db.Item_Type.Find(id);
            if (item_Type == null)
            {
                return HttpNotFound();
            }
            return View(item_Type);
        }

        // POST: Item_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item_Type item_Type = db.Item_Type.Find(id);
            db.Item_Type.Remove(item_Type);
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
