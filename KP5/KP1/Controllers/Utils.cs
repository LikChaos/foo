using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KP1.Models;

namespace KP1.Controllers
{
    public class Utils
    {
        private static ShopDBEntities db = new ShopDBEntities();

        public static void resetStatus(int id_quest)
        {
            
        }

        public static int countInQuest(int id_item,int id_quest)
        {
            int answer = 0;
            foreach (var item in db.Quest_Item.Where(u => u.Id_item == id_item && u.Id_quest == id_quest))
            {
                answer += item.count;
            }
            return answer;
        }

        public static int countFree(int id_item)//что-то уже не так
        {
            int answer = 0;
            Item r = db.Item.Find(id_item);
            if (r!=null || r.count != 0)//ну мало-ли
            {
                foreach (var item in db.Quest_Item.Where(u => u.Id_item == id_item))
                {
                    answer += item.count;
                }
                answer = r.count - answer;
            }
            return answer<0?0:answer;
        }

        public static decimal QuestPrise(int id_quest)
        {
            decimal answer=0;
            foreach (var item in db.Quest_Item.Where(u => u.Id_quest==id_quest))
            {
                answer += item.Item.price * item.count;
            }
            return answer;
        }

        public static int ItemUsed(int id_item)
        {
            int answer = 0;
            foreach (var item in db.Quest_Item.Where(u => u.Quest.State<2))
            {
                answer += item.countIn;
            }
            return answer;
        }
    }
}