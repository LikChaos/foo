using KP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace KP1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = "";

                        using (ShopDBEntities entities = new ShopDBEntities())
                        {
                            User user = entities.User.SingleOrDefault(u => u.Login == username);
                            List<User_Role> AllCombo = entities.User_Role.Where( u=> u.Id_User == user.Id).ToList();
                            List<Role> roleNeed = new List<Role>();
                            foreach (var item in AllCombo)
                            {
                                Role role = entities.Role.SingleOrDefault(u => u.Id == item.Id_Role);
                                roleNeed.Add(role);
                            }
                            foreach (var item in roleNeed)
                            {
                                if (roles != "")
                                {
                                    roles += ";";
                                }
                                roles += item.Role1;
                            }
                        }


                        //Let us set the Pricipal with our user specific details
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));

                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }

        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (ShopDBEntities entities = new ShopDBEntities())
                        {
                            User user = entities.User.SingleOrDefault(u => u.Login == username);
                            List<User_Role> AllCombo = entities.User_Role.Where(u => u.Id_User == user.Id).ToList();
                            List<Role> roleNeed = new List<Role>();
                            foreach (var item in AllCombo)
                            {
                                Role role = entities.Role.SingleOrDefault(u => u.Id == item.Id_Role);
                                roleNeed.Add(role);
                            }
                            foreach (var item in roleNeed)
                            {
                                if (roles != "")
                                {
                                    roles += ";";
                                }
                                roles += item.Role1;
                            }
                        }
                        //let us extract the roles from our own custom cookie


                        //Let us set the Pricipal with our user specific details
                        e.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {
                        //somehting went wrong
                    }
                }
            }
        }
    }
}
