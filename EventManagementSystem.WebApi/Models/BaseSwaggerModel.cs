using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagementSystem.WebApi.Models
{
    public class BaseSwaggerModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}