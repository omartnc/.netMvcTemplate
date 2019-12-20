using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using EventManagementSystem.Entity.Model.Common;


namespace EventManagementSystem.Entity.Model.Authorization
{
    public class Role : BaseEntity
    {
        public Role()
        {
        }

        public string Name { get; set; }
        public string Description { get; set; }
        

        public virtual ICollection<RoleModule> RoleModules { get; set; }
    }
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
        }
    }
}
