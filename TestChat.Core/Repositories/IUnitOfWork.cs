using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestChat.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository IUserRepository { get; }
        IUserChatRepository IUserChatRepository { get; }

        Task<int> CommitAsync();
        int Commit();
    }
}
