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
    public class QuestsController : Controller
    {
        private ShopDBEntities db = new ShopDBEntities();

        // GET: Quests
        public ActionResult Index()
        {
            return View(db.Quest.ToList());
        }

        // GET: Quests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id_quest = id;
            ViewBag.Items= db.Quest_Item.Where(u => u.Id_quest == (int)id).ToList();
            ViewBag.Barcode_Item = new SelectList(db.Item, "Id", "barcode");
            Quest quest = db.Quest.Find(id);
            if (quest == null)
            {
                return HttpNotFound();
            }
            return View(quest);
        }

        // GET: Quests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quests/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,key")] Quest quest)
        {
            if (ModelState.IsValid)
            {
                DateTime r = DateTime.Now;
                quest.Date = r;
                quest.LastEdit = r;
                quest.State = 1;
                db.Quest.Add(quest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quest);
        }

        // GET: Quests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quest quest = db.Quest.Find(id);
            if (quest == null)
            {
                return HttpNotFound();
            }
            return View(quest);
        }

        // POST: Quests/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,key,Date,LastEdit,State")] Quest quest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quest);
        }

        // GET: Quests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quest quest = db.Quest.Find(id);
            if (quest == null)
            {
                return HttpNotFound();
            }
            return View(quest);
        }

        // POST: Quests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Quest quest = db.Quest.Find(id);
            List < Quest_Item > foo= quest.Quest_Item.ToList();
            foreach (var item in foo)
            {
                db.Quest_Item.Remove(item);
            }
            db.Quest.Remove(quest);
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

        public ActionResult PayQuest(int? id)
        {
            var customer = db.Quest.Where(c => c.Id == id).FirstOrDefault();
            customer.State = 2;
            foreach (var item in db.Quest_Item.Where(u => u.Id_quest==id))
            {
                item.Item.count -= item.count;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult AddItemInQuest(int? id_quest , int? Item_count, int? Barcode_Item)//int? id_item,int? id_quest
        {
            if (id_quest == null || Barcode_Item == null)
            {return RedirectToAction("Index", "Quests");}
            if (Item_count==null)
            { Item_count =1;}

            db.Quest.Find((int)id_quest).LastEdit = DateTime.Now;

            int countin = Utils.countFree((int)Barcode_Item) >= Item_count? (int)Item_count : Utils.countFree((int)Barcode_Item);
            Quest_Item foo = new Quest_Item()
            { Id_item = (int)Barcode_Item, Id_quest = (int)id_quest, count = (int)Item_count, countIn = countin };
            if (foo.count != foo.countIn)
            {
                db.Quest.Find((int)id_quest).State = 0;
            }
            db.Quest_Item.Add(foo);
            
            db.SaveChanges();
            return RedirectToAction("Details" , "Quests" , new { id = id_quest }); //id_quest
        }


        public ActionResult DeleteItemFromQuest(int? id_questitem)
        {
            if (id_questitem == null)
            {
                return RedirectToAction("Index", "Quests");
            }

            Quest_Item foo = db.Quest_Item.Find(id_questitem);
            db.Quest_Item.Remove(foo);
            db.Quest.Find(foo.Id_quest).LastEdit = DateTime.Now;
            //updateState-для поиска по пректу
            //блок обновления состояния. да я знаю надо-бы вытащить в функцию
            //но что-то когда вытаскиваю не нравиться. позже может быть перетащю в методы класса
            Quest r = db.Quest.Find(foo.Id_quest);
            int state = 1;
            foreach (var item in r.Quest_Item)
            {
                if (item.count > item.countIn)
                {
                    state = 0;
                    break;
                }
            }
            r.State = state;
            //его окончание
            db.SaveChanges();

            return RedirectToAction("Details", "Quests", new { id = foo.Id_quest });
        }

        public ActionResult Sales(DateTime? fromDate, DateTime? toDate)
        {
            List<Quest> answer = new List<Quest>();
            decimal TotalPrice = 0;
            
            if (fromDate != null && toDate != null)
            {
                foreach (var item in db.Quest.Where(u => u.State==2))
                {
                    if (((DateTime)item.LastEdit).CompareTo(fromDate)>0 || ((DateTime)item.LastEdit).CompareTo(toDate) < 1)
                    {
                        answer.Add(item);
                        TotalPrice += Utils.QuestPrise(item.Id);
                    }
                }
            }
            ViewBag.TotalPrice = TotalPrice;
            return View(answer);
        }

        public ActionResult Notify(int? id)
        {
            db.Quest.Find(id).State = 1;
            return RedirectToAction("Index");
        }
    }
}
