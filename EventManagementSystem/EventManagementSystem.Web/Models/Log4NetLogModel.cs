using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventManagementSystem.Entity.Log4NetLog;

namespace EventManagementSystem.Web.Models
{
    public class Log4NetLogModel
    {
        public List<Log4NetLog> Log4NetLogs { get; set; }
    }
}