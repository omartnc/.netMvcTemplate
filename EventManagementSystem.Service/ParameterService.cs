using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Context;
using EventManagementSystem.Entity.Infrastructure;
using EventManagementSystem.Entity.Model.Parameter;

namespace EventManagementSystem.Service
{
    public class ParameterService:BaseService
    {
        

        public string GetValueByKey(string key)
        {
            return ParameterRepository
                .Get(r => r.Key == key)
                .Value;
        }
        public Parameter Get(string key)
        {
            return ParameterRepository
                .Get(r => r.Key == key);
        }
        public void SetValueByKey(string key, string value)
        {
            var item = Get(key);
            item.Value = value;
            Update(item);
        }
        public List<Parameter> GetAll(int parameterGroupId)
        {
            return ParameterRepository
                .GetAll(r => r.ParameterGroupId == parameterGroupId)
                .ToList();
        }
        public List<Parameter> GetAll()
        {
            return ParameterRepository
                .GetAll()
                .ToList();
        }
        public void Update(Parameter item)
        {
            ParameterRepository.Update(item);
        }
        public void Add(Parameter item)
        {
            ParameterRepository.Add(item);
        }
    }
}
