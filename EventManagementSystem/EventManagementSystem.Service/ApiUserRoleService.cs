using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Model.Authorization;
using EventManagementSystem.Service.Helper;

namespace EventManagementSystem.Service
{
    public class ApiUserRoleService : BaseService
    {
        public List<int> permitted_access_modules;


        public bool GetUserRole(int moduleId, int userId)
        {
            if (permitted_access_modules == null || permitted_access_modules.Count == 0)
            {

                permitted_access_modules = UserRoleCookie.CookieGet();
                if (permitted_access_modules == null || permitted_access_modules.Count == 0)
                {
                    // user'ın sahip oldugu authları cekiyor.
                    var user_all_details = UserRoleRepository.GetAll(r => r.UserId == userId && !r.IsDeleted && r.IsActive).ToList();
                    // bu authlar icerisinden modulleri ekliyor
                    permitted_access_modules = user_all_details.Where(r => r.ModuleId != null && r.RoleId == null && r.Module.IsActive && !r.Module.IsDeleted).Select(r => r.ModuleId.Value).ToList();
                    // sahip oldugu grupları alıyor.
                    var groups = user_all_details.Where(r => r.ModuleId == null && r.RoleId != null).Select(r => r.RoleId).ToList();
                    // o grupların sahip oldukları modulleri ekliyor.
                    permitted_access_modules.AddRange(RoleModuleRepository.GetAll(r => groups.Contains(r.RoleId.Value)).Select(r => r.ModuleId.Value).ToList());
                    
                }


                return permitted_access_modules.Contains(moduleId);
                
            }


            return permitted_access_modules.Contains(moduleId);
        }

        public UserRole Get(int id)
        {
            return UserRoleRepository
                .Get(r => r.Id == id && !r.IsDeleted);
        }
        public List<UserRole> GetAll(int userId)
        {
            return UserRoleRepository
                .GetAll(r => r.UserId == userId && !r.IsDeleted)
                .ToList();
        }
        public void Update(UserRole item)
        {
            UserRoleRepository.Update(item);
        }
        public void Add(UserRole item)
        {
            UserRoleRepository.Add(item);
        }
        public void Delete(int id)
        {
            UserRoleRepository.Delete(id);
        }
        public void DeleteReal(int id)
        {
            UserRoleRepository.DeleteReal(id);
        }
    }
}
