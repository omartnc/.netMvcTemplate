using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Context;
using EventManagementSystem.Entity.Infrastructure;
using EventManagementSystem.Entity.Log4NetLog;
using EventManagementSystem.Entity.Model.Authorization;
using EventManagementSystem.Entity.Model.Common;
using EventManagementSystem.Entity.Model.File;
using EventManagementSystem.Entity.Model.Parameter;

namespace EventManagementSystem.Service
{
    public class BaseService
    {
        #region Common

        internal IRepository<User> UserRepository;

        #endregion
        #region Authorization

        internal IRepository<RoleModule> RoleModuleRepository;
        internal IRepository<Role> RoleRepository;
        internal IRepository<Module> ModuleRepository;
        internal IRepository<UserRole> UserRoleRepository;
        #endregion
        #region File
        internal IRepository<File> FileRepository;
        #endregion
        #region Parameter
        internal IRepository<Parameter> ParameterRepository;
        internal IRepository<ParameterGroup> ParameterGroupRepository;
        #endregion
        #region Log4NetLog
        internal IRepository<Log4NetLog> Log4NetLogRepository;
        #endregion
        public BaseService()
        {

            var dbContext = new ModelContext();
            #region Common

            UserRepository = new Repository<User>(dbContext);

            #endregion
            #region Authorization

            RoleModuleRepository = new Repository<RoleModule>(dbContext);
            RoleRepository = new Repository<Role>(dbContext);
            ModuleRepository = new Repository<Module>(dbContext);
            UserRoleRepository = new Repository<UserRole>(dbContext);

            #endregion
            #region File

            FileRepository = new Repository<File>(dbContext);

            #endregion
            #region Parameter

            ParameterRepository = new Repository<Parameter>(dbContext);
            ParameterGroupRepository = new Repository<ParameterGroup>(dbContext);

            #endregion
            #region Log4NetLog

            Log4NetLogRepository = new Repository<Log4NetLog>(dbContext);

            #endregion
        }
    }
}
