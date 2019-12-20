using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventManagementSystem.Entity.Model.Common;

namespace EventManagementSystem.Web.Models
{
    public class HomeModel : BaseModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public int UserLastLicenseDay { get; set; }
        public int UserProjectConut { get; set; }
        public int UserRemainingProjectConut { get; set; }
        public AdminDashboard AdminDashboard { get; set; } = new AdminDashboard();
        public int ContactsSearchResultCount { get; set; }
        public int ContentSearchResultCount { get; set; }
        public int FileSearchResultCount { get; set; }
        public string Keyword { get; set; }
        public string SiteTitle { get; set; }
        public int PreviousPage { get; set; }
        public int NextPage { get; set; }
        public DateTime ChosenProtocolDay { get; set; }
        public int Page { get; set; }
        public int TotalPage { get; set; }
    }
}