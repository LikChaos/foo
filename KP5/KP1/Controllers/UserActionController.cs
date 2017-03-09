using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KP1.Models;

namespace KP1.Controllers
{
    public class UserActionController : Controller
    {
        ShopDBEntities db = new ShopDBEntities();

        // GET: UserAction
        public ActionResult Quest()
        {
            List<Quest_Item> answer = null;
            if (Session["QuestID"] != null)
            {
                int id = (int)Session["QuestID"];
                ViewBag.Quest = db.Quest.Find(id);
                answer = db.Quest_Item.Where(u => u.Id_quest == id).ToList();
            }
            return View(answer);
        }
        public ActionResult EditQuest()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuest([Bind(Include = "Id,key")] Quest quest)
        {
            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                using (ShopDBEntities entities = new ShopDBEntities())
                {
                    int Id = quest.Id;
                    string key = quest.key;
                    bool QuestValid = entities.Quest.Any(u => u.Id == Id && u.key == key && u.State != 2);
                    // User found in the database
                    if (QuestValid)
                    {
                        Session["QuestID"] = Id;
                        return RedirectToAction("Quest", "UserAction");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Извините но подобного заказа нету.");
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View();
        }

        public ActionResult CreateQuest()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateQuest([Bind(Include = "Id,key")] Quest quest)
        {
            if (ModelState.IsValid)
            {
                DateTime r = DateTime.Now;
                quest.Date = r;
                quest.LastEdit = r;
                db.Quest.Add(quest);
                quest.State = 1;
                db.SaveChanges();
                Session["QuestID"] = quest.Id;
                return RedirectToAction("Quest");
            }
            return View(quest);
        }

        public ActionResult Delete(int id_Quest_Item)
        {
            Quest_Item foo =db.Quest_Item.Find(id_Quest_Item);
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
            return RedirectToAction("Quest");
        }
    }
}