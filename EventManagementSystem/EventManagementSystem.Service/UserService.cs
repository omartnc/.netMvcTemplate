using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Model.Common;
using EventManagementSystem.Helper;

namespace EventManagementSystem.Service
{
    public class UserService:BaseService
    {
        public UserService()
        {
        }

        public User Get()
        {
            try
            {
                var contextUser = HttpContextHelper.HttpContextUsername;
                if (string.IsNullOrEmpty(contextUser))
                    return null;
                return int.TryParse(contextUser, out var userId) ? Get(userId) : GetByUsername(contextUser);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public User Get(int id)
        {
            return UserRepository.GetById(id);
        }
        public User Get(string email, string password)
        {
            return UserRepository.Get(r => r.Email == email && r.Password == password && r.IsActive && !r.IsDeleted);
        }
        public User GetByEmail(string email)
        {
            return UserRepository.Get(r => (r.Email == email || r.Username == email) && !r.IsDeleted && r.IsActive);
        }
        public User GetByMailConfirmationCode(string MailConfirmationCode)
        {
            return UserRepository.Get(r => (r.MailConfirmationCode == MailConfirmationCode) && !r.IsDeleted);
        }
        public User GetByChangePasswordCode(string ChangePasswordCode)
        {
            return UserRepository.Get(r => (r.ChangePasswordCode == ChangePasswordCode) && !r.IsDeleted);
        }
        public User GetByTCKN(string TCKN)
        {
            return UserRepository.Get(r => (r.TCKN == TCKN) && r.IsActive && !r.IsDeleted);
        }
        public User GetByUsername(string username)
        {
            return UserRepository.Get(r => r.Username == username || r.Email == username && r.IsActive && !r.IsDeleted);
        }

        public bool FirstLogin(int id)
        {
            var newUser = UserRepository.GetAll(r => r.IsInitialized == null && r.IsFirstLogin == null && r.IsActive == true && r.IsDeleted == false && r.MailConfirmation == true && r.Id == id).FirstOrDefault();
            if (newUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public User GetByUserFullName(string UserFullName)
        {
            return UserRepository.Get(r => r.FullName == UserFullName && r.IsActive && !r.IsDeleted);
        }
        public List<int> GetAllIds()
        {
            return UserRepository
                .GetAll().Where(r => r.IsActive && r.IsTestUser == false && !r.IsDeleted).Select(x => x.Id).OrderBy(x => x)
                .ToList();
        }

        public List<User> GetAllActiveOrderByName()
        {
            return UserRepository
                .GetAll().Where(r => r.IsActive && r.IsTestUser == false && !r.IsDeleted)
                .OrderBy(x => x.FullName)
                .ToList();
        }
        public List<User> GetAllOrderByName()
        {
            return UserRepository
                .GetAll().Where(r => !r.IsDeleted).OrderByDescending(x => x.CreationDate)
                .ThenBy(x => x.FullName)
                .ToList();
        }
        public List<User> GetAllOrderByDescendingLastLoginDate()
        {
            return UserRepository
                .GetAll().Where(r => !r.IsDeleted && r.LastLoginDate.HasValue).OrderByDescending(x => x.LastLoginDate)
                .ThenBy(x => x.FullName)
                .ToList();
        }
        /// <summary>
        /// Online olan user dışındaki diğer userleri getirir
        /// </summary>
        /// <param name="OnlineUserId">Online olan userin id si verilir</param>
        /// <returns></returns>
        public List<User> GetAllNotOnlineUser(int OnlineUserId)
        {
            return UserRepository
                .GetAll().Where(x => x.Id != OnlineUserId && x.IsActive == true).Where(r => r.IsTestUser == false && !r.IsDeleted)

                .OrderByDescending(r => r.Order).ThenBy(x => x.FullName)
                .ToList();
        }
        public int GetAllCount()
        {
            return UserRepository.GetAll().Count(r => r.IsActive && r.IsTestUser == false && !r.IsDeleted);
        }

        public int GetAllCount(string keyword)
        {
            return UserRepository.GetAll().Count(r => r.IsTestUser == false && !r.IsDeleted && r.IsActive == true && r.FullName.Contains(keyword));
        }
        public void Add(User item)
        {
            UserRepository.Add(item);
        }

        public void Delete(int id)
        {
            UserRepository.Delete(id);
        }
        public List<User> Search()
        {
            return UserRepository
                .GetAll().Where(r => r.IsActive == true).Where(r => r.IsTestUser == false && !r.IsDeleted).OrderByDescending(r => r.Order).ThenBy(x => x.FullName)
                .ToList();
        }

        public List<User> GetAllAddedUsersDescendingCreationDate()
        {
            return UserRepository
                .GetAll().Where(r => !r.IsDeleted).OrderByDescending(r => r.CreationDate).ThenBy(x => x.FullName)
                .ToList();
        }
        public List<User> GetAllThisWeekAddedUsers()
        {
            var DateTimeWeek = DateTime.Now.AddDays(-7);
            return UserRepository
                .GetAll().Where(r => !r.IsDeleted && (r.CreationDate >= DateTimeWeek)).OrderByDescending(r => r.CreationDate).ThenBy(x => x.FullName)
                .ToList();
        }
        public void Update(User item)
        {
            UserRepository.Update(item);
        }

        
        public DataTablesModel.DataTableReturnModel GetAllForDatatables(DataTablesModel.DataTableAjaxPostModel model)
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

            var data = UserRepository.GetAll().Where(r => r.IsActive && r.IsTestUser == false && !r.IsDeleted).Select(x => new
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                CreationDate = x.CreationDate,
                AuthorizationCount = x.UserRoles.Count(r => r.RoleId != null && !r.IsDeleted),
                ModuleCount = x.UserRoles.Count(r => r.ModuleId != null && !r.IsDeleted)
            });

            var totalResultsCount = data.Count();
            var filteredResultsCount = data.Count();

            if (!string.IsNullOrEmpty(searchBy))
            {
                data = data.Where(r => r.FullName.Contains(searchBy));
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
