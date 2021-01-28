using TestChat.Core.Models.EntityModels;
using TestChat.Core.Repositories;
using TestChat.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestChat.Data.Repositories
{
    public class UserChatRepository: Repository<UserChats>, IUserChatRepository
    {
        public UserChatRepository(ChatDbContext context) : base(context)
        {
        }
    }
}
