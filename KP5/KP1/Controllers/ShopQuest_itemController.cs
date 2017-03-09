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
    public class ShopQuest_itemController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: ShopQuest_item
        public ActionResult Index()
        {
            var shopQuest_item = db.ShopQuest_item.Include(s => s.Item).Include(s => s.ShopQuest);
            return View(shopQuest_item.ToList());
        }

        // GET: ShopQuest_item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopQuest_item shopQuest_item = db.ShopQuest_item.Find(id);
            if (shopQuest_item == null)
            {
                return HttpNotFound();
            }
            return View(shopQuest_item);
        }

        // GET: ShopQuest_item/Create
        public ActionResult Create()
        {
            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode");
            ViewBag.Id_ShopQuest = new SelectList(db.ShopQuest, "Id", "Id");
            return View();
        }

        // POST: ShopQuest_item/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Item,Id_ShopQuest,Count")] ShopQuest_item shopQuest_item)
        {
            if (ModelState.IsValid)
            {
                db.ShopQuest_item.Add(shopQuest_item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode", shopQuest_item.Id_Item);
            ViewBag.Id_ShopQuest = new SelectList(db.ShopQuest, "Id", "Id", shopQuest_item.Id_ShopQuest);
            return View(shopQuest_item);
        }

        // GET: ShopQuest_item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopQuest_item shopQuest_item = db.ShopQuest_item.Find(id);
            if (shopQuest_item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode", shopQuest_item.Id_Item);
            ViewBag.Id_ShopQuest = new SelectList(db.ShopQuest, "Id", "Id", shopQuest_item.Id_ShopQuest);
            return View(shopQuest_item);
        }

        // POST: ShopQuest_item/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Item,Id_ShopQuest,Count")] ShopQuest_item shopQuest_item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shopQuest_item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode", shopQuest_item.Id_Item);
            ViewBag.Id_ShopQuest = new SelectList(db.ShopQuest, "Id", "Id", shopQuest_item.Id_ShopQuest);
            return View(shopQuest_item);
        }

        // GET: ShopQuest_item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopQuest_item shopQuest_item = db.ShopQuest_item.Find(id);
            if (shopQuest_item == null)
            {
                return HttpNotFound();
            }
            return View(shopQuest_item);
        }

        // POST: ShopQuest_item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShopQuest_item shopQuest_item = db.ShopQuest_item.Find(id);
            db.ShopQuest_item.Remove(shopQuest_item);
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
