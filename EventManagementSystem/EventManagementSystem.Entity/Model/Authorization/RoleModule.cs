using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using EventManagementSystem.Entity.Model.Common;


namespace EventManagementSystem.Entity.Model.Authorization
{
    public class RoleModule : BaseEntity
    {
        public RoleModule()
        {
        }

        public int? RoleId { get; set; }
        public virtual Role Role { get; set; }
        public int? ModuleId { get; set; }
        public virtual Module Module { get; set; }
    }
    public class RoleModuleMap : EntityTypeConfiguration<RoleModule>
    {
        public RoleModuleMap()
        {
            HasOptional(r => r.Role).WithMany(r => r.RoleModules).HasForeignKey(r => r.RoleId).WillCascadeOnDelete(false);
            HasOptional(r => r.Module).WithMany().HasForeignKey(r => r.ModuleId).WillCascadeOnDelete(false);
        }
    }
}
