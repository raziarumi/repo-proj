using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByName(string name);    
    }
}
