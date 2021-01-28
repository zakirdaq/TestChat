using TestChat.Core.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestChat.Core.Services
{
    public interface IUserChatService
    {
        Task<UserChats> Create(UserChats obj);
        Task<UserChats> Update(UserChats obj);
        Task<UserChats> Update(UserChats objToBeUpdated, UserChats obj);
        Task<UserChats> GetById(Guid id);
        Task<IEnumerable<UserChats>> GetByUserId(Guid userId);
        Task Remove(UserChats obj);
    }
}
