using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventManagementSystem.Entity.Model.Common;

namespace EventManagementSystem.Web.Models
{
    public class AdminDashboard
    {
        public int ThisWeekAddedUserCount { get; set; }
        public int ThisWeekAddedProjectCount { get; set; }
        public int ActiveLicenseCount { get; set; }
        public int TotalUserCount { get; set; }
        public int TotalProjectCount { get; set; }
        public int TotalLicenseCount { get; set; }
        public List<User> LastAddedUsers { get; set; } = new List<User>();
        public List<User> LastLoginUsers { get; set; } = new List<User>();
    }
}