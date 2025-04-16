using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Commom;
using Ambev.DeveloperEvaluation.Application.Interfaces;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Handler for processing GetUserCommand requests
/// </summary>
public class GetUserHandler : IRequestHandler<GetUserQuery, GetUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IValidator<GetUserQuery> _validator;

    public GetUserHandler(
        IUserRepository userRepository,
        IUserQueryRepository userQueryRepository,
        IMapper mapper,
        IValidator<GetUserQuery> validator)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _userQueryRepository = userQueryRepository ?? throw new ArgumentNullException(nameof(userQueryRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<GetUserResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        // Validate the request
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Attempt to retrieve the user from the cache
        var user = await GetUserFromCacheOrRepositoryAsync(request.Id, cancellationToken);

        // Map the user entity to the result DTO
        return _mapper.Map<GetUserResult>(user);
    }

    private async Task<User> GetUserFromCacheOrRepositoryAsync(Guid userId, CancellationToken cancellationToken)
    {
        var parameters = new QueryParams<User>
        {
            Filter = u => u.Id.Equals(userId)
        };

        var userCache = await _userQueryRepository.QueryAsync(parameters, cancellationToken);
        return userCache?.FirstOrDefault()
            ?? await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found");
    }
}
