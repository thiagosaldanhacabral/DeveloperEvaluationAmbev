using Ambev.DeveloperEvaluation.Application.Commom;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.Extensions.Caching.Distributed;

namespace Ambev.DeveloperEvaluation.Application.Users
{
    public class UserQueryRepository : CacheableQueryRepository<User>, IUserQueryRepository
    {
        public UserQueryRepository(DefaultContext context, IDistributedCache cache)
            : base(context, cache) { }
    }
}
