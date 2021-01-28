using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestChat.Core.Repositories;
using TestChat.Data.Contexts;

namespace TestChat.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Declaration & Construction & Dispose & Commit
        private readonly ChatDbContext _context;

        public UnitOfWork(ChatDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }
        #endregion  Declaration & Construction & Dispose & Commit

        #region PrivateRepositoryEntity
        private UserRepository _userRepository;
        private UserChatRepository _UserChatRepository;
        #endregion PrivateRepositoryEntity

        #region RepositoryEntityFromInterface
        public IUserRepository IUserRepository => _userRepository = _userRepository ?? new UserRepository(_context);
        public IUserChatRepository IUserChatRepository => _UserChatRepository = _UserChatRepository ?? new UserChatRepository(_context);
        #endregion RepositoryEntityFromInterface
    }
}
