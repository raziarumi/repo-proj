using Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDataContext context;
        public UserRepository(ApplicationDataContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<User> GetByName(string name)
        {
            var user =  (from usr in context.Users
                        where usr.Name == name
                        select usr);
            return await user.FirstOrDefaultAsync();
        }

    }
}
