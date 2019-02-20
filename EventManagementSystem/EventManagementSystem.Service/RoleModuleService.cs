using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Model.Authorization;

namespace EventManagementSystem.Service
{
    public class RoleModuleService:BaseService
    {
        public RoleModule Get(int id)
        {
            return RoleModuleRepository
                .Get(r => r.Id == id && !r.IsDeleted);
        }
        public List<RoleModule> GetAll(int roleId)
        {
            return RoleModuleRepository
                .GetAll(r => r.RoleId == roleId && !r.IsDeleted)
                .ToList();
        }
        public void Update(RoleModule item)
        {
            RoleModuleRepository.Update(item);
        }
        public void Add(RoleModule item)
        {
            RoleModuleRepository.Add(item);
        }
        public void Delete(int id)
        {
            RoleModuleRepository.Delete(id);
        }
    }
}
