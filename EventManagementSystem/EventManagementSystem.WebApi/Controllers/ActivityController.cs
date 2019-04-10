using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EventManagementSystem.Entity.Model.Parameter;
using EventManagementSystem.Service;
using EventManagementSystem.WebApi.Helpers;
using EventManagementSystem.WebApi.Models;

namespace EventManagementSystem.WebApi.Controllers
{
    public class ActivityController : BaseController
    {
        [AuthorizeUser(Action = "List")]
        [ResponseType(typeof(List<ParameterSwaggerModel>))]
        public IHttpActionResult getParameters()
        {
            try
            {

                var list = new ParameterService().GetAll(1).Select(x => new Parameter
                {
                    Id = x.Id,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted
                });
                Logger.Info(CurrentUser.Id + " idli kullanıcı  City listesi gösterimi yaptı.");
                return Ok(list);
            }
            catch (Exception e)
            {
                Logger.Error("Hata oluştu - " + new StackTrace().GetFrame(0).GetMethod().Name, e);
                return BadRequest();
            }
        }
    }
}
