using Ambev.DeveloperEvaluation.Application.Commom;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Interfaces
{
    public interface IUserQueryRepository
    {
        Task<IEnumerable<User>> QueryAsync(QueryParams<User> parameters, CancellationToken cancellationToken = default);
    }
}
