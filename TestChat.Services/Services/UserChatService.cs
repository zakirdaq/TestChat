using TestChat.Core.Models.EntityModels;
using TestChat.Core.Repositories;
using TestChat.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestChat.Services.Services
{
    public class UserChatService : IUserChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserChatService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<UserChats> Create(UserChats obj)
        {
            _unitOfWork.IUserChatRepository.Add(obj);

            await _unitOfWork.CommitAsync();

            return obj;
        }

        public async Task Remove(UserChats obj)
        {
            _unitOfWork.IUserChatRepository.Remove(obj);

             await _unitOfWork.CommitAsync();
        }

        public async Task<UserChats> GetById(Guid id)
        {
            return await _unitOfWork.IUserChatRepository.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<UserChats>> GetByUserId(Guid senderId, Guid recieverId)
        {
            return await _unitOfWork.IUserChatRepository.FindAsync(a => (a.SenderId == senderId && a.RecieverId == recieverId) || (a.SenderId == recieverId && a.RecieverId == senderId), inc => inc.Reciever, inc2=>inc2.Sender);
        }

        public async Task<UserChats> Update(UserChats obj)
        {
            await _unitOfWork.CommitAsync();
            return obj;
        }

        public async Task<UserChats> Update(UserChats objToBeUpdated, UserChats obj)
        {
            await _unitOfWork.CommitAsync();
            return obj;
        }
    }
}
