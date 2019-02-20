using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using EventManagementSystem.Entity.Model.Common;


namespace EventManagementSystem.Entity.Model.Parameter
{
    public class Parameter : BaseEntity
    {
        public Parameter()
        {

        }
        public string Key { get; set; }
        public string Value { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int? ParameterGroupId { get; set; }
        public virtual ParameterGroup ParameterGroup { get; set; }
    }
    public class ParameterMap : EntityTypeConfiguration<Parameter>
    {
        public ParameterMap()
        {
            HasOptional(r => r.User).WithMany(r => r.Parameters).HasForeignKey(r => r.UserId).WillCascadeOnDelete(false);
            HasOptional(r => r.ParameterGroup).WithMany(r=>r.Parameters).HasForeignKey(r => r.ParameterGroupId).WillCascadeOnDelete(false);
        }
    }
}
