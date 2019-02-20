using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EventManagementSystem.Entity.Model.Common
{
    public class BaseEntity: Infrastructure.Entity
    {
        public BaseEntity()
        {

        }
        [Key]
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
