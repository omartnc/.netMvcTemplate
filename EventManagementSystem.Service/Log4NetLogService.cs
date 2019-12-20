using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Log4NetLog;
using EventManagementSystem.Helper;

namespace EventManagementSystem.Service
{

    public class Log4NetLogService : BaseService
    {
        public List<Log4NetLog> GetAll()
        {
            return Log4NetLogRepository
                .GetAll(r => r.IsDeleted == false && r.Level == "INFO")
                .ToList();
        }

        public DataTablesModel.DataTableReturnModel GetAllForDatatables(DataTablesModel.DataTableAjaxPostModel model, string date)
        {
            var searchBy = model.search?.value;
            var take = model.length;
            var skip = model.start;

            var sortBy = "Id";
            var sortDir = "asc";
            if (model.order != null)
            {
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower();
            }



            var data = GetAll().Where(r => !r.IsDeleted && (string.IsNullOrEmpty(date) || r.CreationDate.ToString("dd.MM.yyyy") == date)).Select(x => new
            {
                IsActive = x.IsActive,
                Id = x.Id,
                CreationDate = x.CreationDate,
                Message = x.Message
            });

            var totalResultsCount = data.Count();
            var filteredResultsCount = data.Count();

            if (!string.IsNullOrEmpty(searchBy))
            {
                data = data.Where(r => r.CreationDate.ToString().Contains(searchBy) || r.Message.ToLower().ToString().Contains(searchBy.ToLower()));
                filteredResultsCount = data.Count();
            }
            data = data.OrderBy($"{sortBy} {sortDir}").Skip(skip).Take(take);

            return new DataTablesModel.DataTableReturnModel
            {
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = data.ToList()
            };

        }
    }
}
