using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventManagementSystem.Entity.Model.Common;
using EventManagementSystem.Service;
using log4net;

namespace EventManagementSystem.WebApi.Controllers
{
    [Authorize]
    public class BaseController : ApiController
    {
        public static readonly ILog Logger = LogManager.GetLogger(System.Environment.UserName);

        public UserService UserService = new UserService();
        public User CurrentUser = null;

        public BaseController()
        {
            log4net.LogicalThreadContext.Properties["IsActive"] = true;
            log4net.LogicalThreadContext.Properties["IsDeleted"] = false;
            log4net.LogicalThreadContext.Properties["CreationDate"] = DateTime.Now;
            CurrentUser = UserService.GetForApi();
        }
    }
}
