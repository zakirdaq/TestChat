 
using System;
using System.Collections.Generic;
using System.Text;
using TestChat.Core.Models.EntityModels;
using TestChat.Core.Repositories;
using TestChat.Data.Contexts;

namespace TestChat.Data.Repositories
{
    public class UserRepository: Repository<Users>, IUserRepository
    {
        public UserRepository(ChatDbContext context): base(context)
        {

        }
    }
}
