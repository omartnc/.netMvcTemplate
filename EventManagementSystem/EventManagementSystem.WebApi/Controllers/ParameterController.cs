using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventManagementSystem.Entity.Model.Parameter;
using EventManagementSystem.Service;
using EventManagementSystem.WebApi.Helpers;

namespace EventManagementSystem.WebApi.Controllers
{
    public class ParameterController : ApiController
    {
        [AuthorizeUser(Action = "List")]
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
                return Ok(list);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }
    }
}
