using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Model.Common;

namespace EventManagementSystem.Entity.Log4NetLog
{
    public class Log4NetLog : BaseEntity
    {
        public DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }

    public class Log4NetLogMap : EntityTypeConfiguration<Log4NetLog>
    {
        public Log4NetLogMap()
        {

        }
    }
}
