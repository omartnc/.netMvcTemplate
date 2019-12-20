using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using EventManagementSystem.Entity.Model.Common;


namespace EventManagementSystem.Entity.Model.Parameter
{
    public class ParameterGroup : BaseEntity
    {
        public ParameterGroup()
        {

        }
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<Parameter> Parameters { get; set; }
    }
    public class ParameterGroupMap : EntityTypeConfiguration<ParameterGroup>
    {
        public ParameterGroupMap()
        {
        }
    }
}
