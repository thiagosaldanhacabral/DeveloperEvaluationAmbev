using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Application.Interfaces;

namespace Ambev.DeveloperEvaluation.Application.Users.CreateUser;

/// <summary>
/// Handler for processing CreateUserCommand requests
/// </summary>
public class CreateUserHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IPasswordHasher passwordHasher,
    IUserCreateRepository userCreateRepository
) : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    /// <summary>
    /// Handles the CreateUserCommand request
    /// </summary>
    /// <param name="command">The CreateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created user details</returns>
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateUserCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingUser = await userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (existingUser != null)
            throw new InvalidOperationException($"User with email {command.Email} already exists");

        var user = mapper.Map<User>(command);
        user.Password = passwordHasher.HashPassword(command.Password);

        var createdUser = await userRepository.CreateAsync(user, cancellationToken);

        var cacheKey = $"User:{createdUser.Id}";
        await userCreateRepository.StoreUserInCacheAsync(createdUser, cacheKey, cancellationToken);

        return mapper.Map<CreateUserResult>(createdUser);
    }
}
