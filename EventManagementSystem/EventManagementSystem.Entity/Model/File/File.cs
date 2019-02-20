using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using EventManagementSystem.Entity.Model.Common;


namespace EventManagementSystem.Entity.Model.File
{
    public class File : BaseEntity
    {
        public File()
        {

        }
        public string Path { get; set; }
        public string Tags { get; set; }
        public DateTime UploadedTime { get; set; }
    }
    public class FileMap : EntityTypeConfiguration<File>
    {
        public FileMap()
        {
        }
    }
}
