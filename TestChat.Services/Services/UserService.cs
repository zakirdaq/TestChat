using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestChat.Core.Models.EntityModels;
using TestChat.Core.Repositories;
using TestChat.Core.Services;

namespace TestChat.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Users> Create(Users objToBeAdd)
        {
            await _unitOfWork.IUserRepository.AddAsync(objToBeAdd);

            await _unitOfWork.CommitAsync();
            return objToBeAdd;
        }

        public bool CheckDuplicateEmail(Guid userid, string email, string type)
        {
            if (type.ToLower() == "edit")
            {
                var data = _unitOfWork.IUserRepository.SingleOrDefault(a => a.Email == email && a.Id != userid);
                if (data != null)
                { return true; }
                else return false;
            }
            else
            {
                var data = _unitOfWork.IUserRepository.SingleOrDefault(a => a.Email == email);
                if (data != null)
                { return true; }
                else return false;
            }
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
        public async Task<IEnumerable<Users>> GetAll()
        {
            return await _unitOfWork.IUserRepository.GetAllAsync();
        }

        public async Task<Users> GetById(Guid id)
        {
            return await _unitOfWork.IUserRepository.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Users> GetByEmail(string email)
        {
            return await _unitOfWork.IUserRepository.SingleOrDefaultAsync(a => a.Email == email);
        }
    }
}
