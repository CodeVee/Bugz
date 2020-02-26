using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await FindAll().OrderBy(user => user.FirstName).ToListAsync();
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await FindByCondition(user => user.Id.Equals(id))
                                .FirstOrDefaultAsync();
            return user;
        }
    }
}