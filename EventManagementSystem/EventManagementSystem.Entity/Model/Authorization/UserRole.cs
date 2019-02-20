using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using EventManagementSystem.Entity.Model.Common;


namespace EventManagementSystem.Entity.Model.Authorization
{
    public class UserRole : BaseEntity
    {
        public UserRole()
        {
        }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int? ModuleId { get; set; }
        public virtual Module Module { get; set; }
        public int? RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
    public class UserRoleMap : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            HasOptional(r => r.Role).WithMany().HasForeignKey(r => r.RoleId).WillCascadeOnDelete(false);
            HasOptional(r => r.Module).WithMany().HasForeignKey(r => r.ModuleId).WillCascadeOnDelete(false);
            HasOptional(r => r.User).WithMany(r => r.UserRoles).HasForeignKey(r => r.UserId).WillCascadeOnDelete(false);
        }
    }
}
