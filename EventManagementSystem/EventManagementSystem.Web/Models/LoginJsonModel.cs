using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagementSystem.Web.Models
{
    public class LoginJsonModel
    {
        public bool IsLogin { get; set; }
        public string Link { get; set; }
        public string ErrorMessage { get; set; }
    }
}