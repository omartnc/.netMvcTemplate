
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using EventManagementSystem.Entity.Model.Common;

namespace EventManagementSystem.Entity.Model.Authorization
{
    public class Module:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public int? ParentModuleId { get; set; }
        public virtual Module ParentModule { get; set; }
        public virtual ICollection<Module> ParentModules { get; set; }

    }
    public class ModuleMap : EntityTypeConfiguration<Module>
    {
        public ModuleMap()
        {
            HasOptional(r => r.ParentModule).WithMany(r=>r.ParentModules).HasForeignKey(r => r.ParentModuleId).WillCascadeOnDelete(false);
        }
    }
}
