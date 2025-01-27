using HealthManager.WebApp.BS.Contracts;
using HealthManager.WebApp.BS.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthManager.WebApp.BS.Repository
{
    internal sealed class UserStatusRepository : RepositoryBase<UserStatus>, IUserStatusRepository
    {
        public UserStatusRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<UserStatus> GetUserStatusByName(string name)
        {
            return await FindByCondition(x => x.StatusName == name, true).FirstOrDefaultAsync();
        }
    }
}
