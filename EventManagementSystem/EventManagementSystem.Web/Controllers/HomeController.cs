using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManagementSystem.Web.Helpers;
using EventManagementSystem.Web.Models;

namespace EventManagementSystem.Web.Controllers
{
    [AuthorizeUser(Action = "Index")]
    public class HomeController : BaseController
    {
        [Route("")]
        [HttpGet]
        public ActionResult Index()
        {
            var model = new HomeModel();
            try
            {
                var user = CurrentUser;

                
                    model.User = user;
                    model.AdminDashboard.ThisWeekAddedUserCount = UserService.GetAllThisWeekAddedUsers().Count;
                    model.AdminDashboard.TotalUserCount = UserService.GetAllCount();
                    model.AdminDashboard.LastAddedUsers = UserService.GetAllOrderByName();
                    model.AdminDashboard.LastLoginUsers = UserService.GetAllOrderByDescendingLastLoginDate().Take(6).ToList();
                



            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }

            return View(model);
        }

        [AuthorizeUser(Action = "Header")]
        public PartialViewResult Header()
        {
            try
            {
                var model = new HomeModel
                {
                    User = CurrentUser
                };
               
                    model.UserRemainingProjectConut = 0;
              
                return PartialView(model);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return PartialView(new HomeModel());
            }
        }

        [AuthorizeUser(Action = "Footer")]
        public PartialViewResult Footer()
        {

            var model = new HomeModel();

            return PartialView(model);
        }

        [AuthorizeUser(Action = "Sidebar")]
        public PartialViewResult Sidebar()
        {
            var model = new HomeModel();
            try
            {

                model.SiteTitle = ParameterService.Get("SiteHeader").Value;
                model.User = CurrentUser;
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return PartialView(model);
        }

        [AuthorizeUser(Action = "Sidebar")]
        public bool ChangeSidebarPozition(bool Pozition)
        {
            try
            {
                var user = UserService.Get(CurrentUser.Id);
                if (user == null)
                    return false;
                user.SidebarPozitionConstant = Pozition;
                UserService.Update(user);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return true;
        }

        [AuthorizeUser(Action = "Sidebar")]
        public PartialViewResult SidebarPozition()
        {
            return PartialView(CurrentUser);

        }

        public PartialViewResult SiteTitle()
        {
            var model = new HomeModel();
            try
            {
                model.SiteTitle = ParameterService.Get("SiteHeader").Value;
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
            }
            return PartialView(model);
        }
    }
}