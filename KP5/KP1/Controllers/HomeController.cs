using KP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KP1.Controllers
{
    public class HomeController : Controller
    {
        ShopDBEntities db = new ShopDBEntities();
        // GET: Home
        

        public ActionResult Index()
        {
            if (User.Identity.Name.ToString() != "")
            {
                return RedirectToAction("Index","WorkerHome");
            }
            return View();
        }

        public ActionResult HowToUse()
        {
            return View();
        }

        public ActionResult ItemScroll()
        {
            if (Session["ItemShowId"] != null)
            {
                int id = (int)Session["ItemShowId"];
                Session["ItemShowId"] = null;
                List<Item> answer = new List<Item>();
                List<Item_Type> buff = db.Item_Type.Where(u => u.Id_ItemType == id).ToList();
                foreach (var item in buff)
                {
                    Item r = db.Item.Single(u => u.Id == item.Id_Item);
                    answer.Add(r);
                }
                return View(answer);
            }
            return View();
        }

        public ActionResult TypeScroll()
        {
            List<ItemType> answer = new List<ItemType>();
            answer = db.ItemType.ToList();
            return View(answer);
        }

        public ActionResult AddItemInQuest(int id_item, int? Item_count)
        {
            int? Id_Quest =(int?) Session["QuestID"];
            if (Item_count != null && Session["QuestID"] != null)
            {
                if (Utils.countInQuest(id_item, (int)Id_Quest) + Item_count <= 10)
                {
                    int countin = Utils.countFree(id_item) >= Item_count? (int)Item_count : Utils.countFree(id_item);
                    Quest_Item foo = new Quest_Item()
                    { Id_item = id_item, Id_quest = (int)Session["QuestID"], count = (int)Item_count, countIn= countin };
                    db.Quest_Item.Add(foo);
                    if (foo.count!=foo.countIn)
                    {
                        db.Quest.Find(Id_Quest).State = 0;
                    }
                    db.Quest.Find(foo.Id_quest).LastEdit = DateTime.Now;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult ShowItems(int? id)//+Опции фильтров и тд и тп
        {
            if (id != null)
            {
                Session["ItemShowId"] = id;
            }
            return RedirectToAction("Index");
        }
    }
}