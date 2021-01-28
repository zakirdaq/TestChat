using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestChat.Core.Models.EntityModels;

namespace TestChat.Core.Services
{
    public interface IUserService
    {
        Task<Users> Create(Users objToBeAdd);
        Task<IEnumerable<Users>> GetAll();
        bool CheckDuplicateEmail(Guid userid, string email, string type);
        Task<Users> GetById(Guid id);
        void Commit();
        Task<Users> GetByEmail(string email);
    }
}
