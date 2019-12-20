using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Model.Authorization;

namespace EventManagementSystem.Service
{
    public class ModuleService:BaseService
    {
        public List<Module> GetAll()
        {
            return ModuleRepository
                .GetAll(r => !r.IsDeleted).ToList();
        }
        public Module Get(string name)
        {
            return ModuleRepository
                .GetAll(r => r.Name == name && !r.IsDeleted)
                .FirstOrDefault();
        }
    }
}
