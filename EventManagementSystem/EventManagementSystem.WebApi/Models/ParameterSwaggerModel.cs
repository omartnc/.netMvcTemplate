using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagementSystem.WebApi.Models
{
    public class ParameterSwaggerModel:BaseSwaggerModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int? UserId { get; set; }
        public int? ParameterGroupId { get; set; }
    }
}