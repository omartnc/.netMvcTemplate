using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventManagementSystem.Entity.Model.Authorization;
using EventManagementSystem.Entity.Model.Common;

namespace EventManagementSystem.Web.Models
{
    public class AuthorizationModel : BaseModel
    {
        public Role Role { get; set; }
        public List<Role> Roles { get; set; }
        public List<int> RoleModules { get; set; }
        public List<int> UserRoles { get; set; }
        public List<int> UserModules { get; set; }
        public List<Module> Modules { get; set; }
        public List<User> Users { get; set; }
        public bool? waitingPayment { get; set; }
    }
}