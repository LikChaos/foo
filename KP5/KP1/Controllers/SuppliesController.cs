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
    public class SuppliesController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: Supplies
        public ActionResult Index()
        {
            var supply = db.Supply.Include(s => s.Provider);
            return View(supply.ToList());
        }

        // GET: Supplies/Details/5
        public ActionResult Details(int? id, int id_provider)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supply supply = db.Supply.Find(id);
            ViewBag.Suppliy_item = db.Supply_Item.Where(u =>u.Id_supply == supply.Id).ToList();
            ViewBag.id_supply = id;
            List<Item> foo=new List<Item>();
            foreach (var item in db.Provider_Item.Where(u => u.Id_provider == id_provider))
            {
                foo.Add(item.Item);
            }
            ViewBag.Barcode_Item = new SelectList(foo, "Id", "barcode");
            if (supply == null)
            {
                return HttpNotFound();
            }
            return View(supply);
        }

        // GET: Supplies/Create
        public ActionResult Create()
        {
            ViewBag.Id_Provider = new SelectList(db.Provider, "Id", "Name");
            return View();
        }

        // POST: Supplies/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Id_Provider,Date")] Supply supply)
        {
            if (ModelState.IsValid)
            {
                supply.State = 0;
                db.Supply.Add(supply);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Provider = new SelectList(db.Provider, "Id", "Name", supply.Id_Provider);
            return View(supply);
        }

        // GET: Supplies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supply supply = db.Supply.Find(id);
            if (supply == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Provider = new SelectList(db.Provider, "Id", "Name", supply.Id_Provider);
            return View(supply);
        }

        // POST: Supplies/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Id_Provider,Date,State")] Supply supply)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supply).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Provider = new SelectList(db.Provider, "Id", "Name", supply.Id_Provider);
            return View(supply);
        }

        // GET: Supplies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supply supply = db.Supply.Find(id);
            if (supply == null)
            {
                return HttpNotFound();
            }
            return View(supply);
        }

        // POST: Supplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supply supply = db.Supply.Find(id);
            db.Supply.Remove(supply);
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

        public ActionResult DeleteItem(int id,int id_supply)
        {
            Supply_Item supply = db.Supply_Item.Find(id);
            db.Supply_Item.Remove(supply);
            db.SaveChanges();
            Supply bur = db.Supply.Single(u => u.Id == id_supply);
            return RedirectToAction("Details", new { id = id_supply, id_provider = bur.Id_Provider });
        }


        public ActionResult AddItemInSupply(int? id_supply, int? Barcode_Item, int? count)
        {
            if (id_supply==null || Barcode_Item==null || count==null)
            {
                return RedirectToAction("Index");
            }
            Supply_Item foo = new Supply_Item() {Id_supply=(int)id_supply, Id_Item= (int)Barcode_Item, Count= (int)count };
            db.Supply_Item.Add(foo);
            db.SaveChanges();
            Supply bur = db.Supply.Single(u=> u.Id== id_supply);
            return RedirectToAction("Details",new {Id = id_supply , id_provider=bur.Id_Provider });
        }

        public ActionResult GetShopQuest(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Supply foo = db.Supply.Find(id);
            foo.State = 3;
            foreach (var item in foo.Supply_Item)
            {
                int count = item.Count;
                db.Item.Find(item.Id_Item).count+= count;
                foreach (var item2 in db.Quest_Item.Where(u => u.Id_item==item.Id_Item && u.Quest.State == 0))
                {
                    int c = item2.count - item2.countIn;
                    if (count >= c)
                    {
                        item2.countIn = item2.count;
                        count -= c;
                        //updateState-для поиска по пректу
                        //блок обновления состояния. да я знаю надо-бы вытащить в функцию
                        //но что-то когда вытаскиваю не нравиться. позже может быть перетащю в методы класса
                        Quest r = db.Quest.Find(item2.Id_quest);
                        int state = 2;
                        foreach (var i in r.Quest_Item)
                        {
                            if (i.count > i.countIn)
                            {
                                state = 0;
                                break;
                            }
                        }
                        r.State = state;
                        //его окончание
                    }
                    else
                    {
                        item2.countIn += count;
                        count = 0;
                        break;
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
