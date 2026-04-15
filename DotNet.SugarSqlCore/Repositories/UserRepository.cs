using DotNet.SugarSqlCore.Entities;
using SugarSqlCore.Abstraction;

namespace DotNet.SugarSqlCore.Repositories
{
    public class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        public UserRepository(ISugarSqlDbContextProvider<ISugarSqlDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
