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
    public class WriteOffsController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: WriteOffs
        public ActionResult Index()
        {
            var writeOff = db.WriteOff.Include(w => w.Item);
            return View(writeOff.ToList());
        }

        // GET: WriteOffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WriteOff writeOff = db.WriteOff.Find(id);
            if (writeOff == null)
            {
                return HttpNotFound();
            }
            return View(writeOff);
        }

        // GET: WriteOffs/Create
        public ActionResult Create()
        {
            ViewData["ErrMessage"] = TempData["ErrMessage"];
            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode");
            return View();
        }

        // POST: WriteOffs/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Item,Count")] WriteOff writeOff)
        {
            if (ModelState.IsValid)
            {
                if (db.Item.Find(writeOff.Id_Item).count - writeOff.Count < Utils.ItemUsed(writeOff.Id_Item))
                {
                    TempData["ErrMessage"] = "Вы пытаетесь списать необходимый товар. Разберитесь с заказами в которых он участвует прежде чем списывать его.";
                    return RedirectToAction("Create");
                }
                writeOff.Date = DateTime.Now;
                db.WriteOff.Add(writeOff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode", writeOff.Id_Item);
            return View(writeOff);
        }

        // GET: WriteOffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WriteOff writeOff = db.WriteOff.Find(id);
            if (writeOff == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode", writeOff.Id_Item);
            return View(writeOff);
        }

        // POST: WriteOffs/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Id_Item,Count")] WriteOff writeOff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(writeOff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Item = new SelectList(db.Item, "Id", "barcode", writeOff.Id_Item);
            return View(writeOff);
        }

        // GET: WriteOffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WriteOff writeOff = db.WriteOff.Find(id);
            if (writeOff == null)
            {
                return HttpNotFound();
            }
            return View(writeOff);
        }

        // POST: WriteOffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WriteOff writeOff = db.WriteOff.Find(id);
            db.WriteOff.Remove(writeOff);
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
