using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Model.Authorization;

namespace EventManagementSystem.Service
{
    public class RoleService:BaseService
    {
        public Role Get(int id)
        {
            return RoleRepository
                .Get(r => r.Id == id);
        }
        public Role GetByName(string Name)
        {
            return RoleRepository
                .Get(r => r.Name == Name);
        }
        public List<Role> GetAll()
        {
            return RoleRepository
                .GetAll()
                .ToList();
        }
        public void Update(Role item)
        {
            RoleRepository.Update(item);
        }
        public void Add(Role item)
        {
            RoleRepository.Add(item);
        }
        public void Delete(int id)
        {
            RoleRepository.Delete(id);
        }
    }
}
