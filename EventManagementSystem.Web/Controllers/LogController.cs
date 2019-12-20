using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManagementSystem.Entity.Log4NetLog;
using EventManagementSystem.Helper;
using EventManagementSystem.Web.Helpers;
using EventManagementSystem.Web.Models;

namespace EventManagementSystem.Web.Controllers
{
    [RoutePrefix("log")]
    public class LogController : BaseController
    {
        [AuthorizeUser(Action = "List")]
        [Route("list")]
        [HttpGet]
        public ActionResult List()
        {
            var model = new Log4NetLogModel
            {
                Log4NetLogs = new List<Log4NetLog>()
            };
            return View(model);
        }


        [AuthorizeUser(Action = "List")]
        [HttpPost]
        public JsonResult Log4NetLogLog4NetLogListQuery(DataTablesModel.DataTableAjaxPostModel model, string date)
        {
            try
            {
                var data = Log4NetLogService.GetAllForDatatables(model, date);
                return Json(data);

            }
            catch (Exception e)
            {
                Logger.Error("Error Occured! - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return Json("");
            }
        }
    }
}