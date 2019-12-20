using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Log4NetLog;
using EventManagementSystem.Entity.Model.Authorization;
using EventManagementSystem.Entity.Model.Common;
using EventManagementSystem.Entity.Model.File;
using EventManagementSystem.Entity.Model.Parameter;

namespace EventManagementSystem.Entity.Context
{
    public class ModelContext : DbContext
    {
        public ModelContext()
            : base("Name=ModelContext")
        {
            Database.SetInitializer<ModelContext>(null);
        }
        #region Common

        public DbSet<User> Users { get; set; }

        #endregion
        #region Authorization

        public DbSet<RoleModule> RoleModules { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        #endregion
        #region File

        public DbSet<File> Files { get; set; }

        #endregion
        #region Parameter

        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<ParameterGroup> ParameterGroups { get; set; }

        #endregion
        #region Log4NetLog

        public DbSet<Log4NetLog.Log4NetLog> Log4NetLogs { get; set; }

        #endregion
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region User

            modelBuilder.Configurations.Add(new UserMap());

            #endregion
            #region Authentication

            modelBuilder.Configurations.Add(new RoleModuleMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new ModuleMap());
            modelBuilder.Configurations.Add(new UserRoleMap());

            #endregion
            #region File

            modelBuilder.Configurations.Add(new FileMap());

            #endregion
            #region Parameter

            modelBuilder.Configurations.Add(new ParameterMap());
            modelBuilder.Configurations.Add(new ParameterGroupMap());

            #endregion
            #region Log4NetLog

            modelBuilder.Configurations.Add(new Log4NetLogMap());

            #endregion
        }
    }
}
