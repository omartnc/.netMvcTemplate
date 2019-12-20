using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using EventManagementSystem.Entity.Model.Authorization;

namespace EventManagementSystem.Entity.Model.Common
{
    public class User:BaseEntity
    {

        public string Username { get; set; }
        public int Order { get; set; } = 9999;
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        
        public string CompanyName { get; set; }
        public string ContractText { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public string TCKN { get; set; }
        public string PhotoPath { get; set; }
        /// <summary>
        /// Mail izin / Bilgi istiyorum
        /// </summary>
        public bool MailPermission { get; set; }

        public bool MailConfirmation { get; set; } = false;
        public string MailConfirmationCode { get; set; } = Guid.NewGuid().ToString().Replace("-", "").ToLower();
        public bool IsChangePassword { get; set; } = false;
        public string ChangePasswordCode { get; set; } = Guid.NewGuid().ToString().Replace("-", "").ToLower();
        public bool IsAdmin { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public bool? IsFirstLogin { get; set; }
        public bool? IsInitialized { get; set; }

        public bool? IsEmployee { get; set; }
        public int? parentUserId { get; set; }

        

        public bool IsTestUser { get; set; } = false;
        public bool SidebarPozitionConstant { get; set; } = false;
        public int FailedLoginCount { get; set; }
        public string SecondTitle { get; set; }

        
        public virtual ICollection<Parameter.Parameter> Parameters { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
        }
    }
}
