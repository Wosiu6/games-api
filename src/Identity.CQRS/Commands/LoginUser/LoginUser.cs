using Ardalis.GuardClauses;
using Domain.Common.Interfaces;
using Domain.Entities;
using Domain.Identity.IdentityProviders;
using Domain.Identity.PasswordHashers;
using Infrastructure.Data.Extensions;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Identity.CQRS.Commands.LoginUser;

public record LoginUserCommand : IRequest<string>
{
    [Required]
    [EmailAddress]
    [DefaultValue("admin@example.com")]
    public string Email { get; set; }

    [Required]
    [DefaultValue("AdminPassword123£")]
    public string Password { get; set; }
}

public class LoginUserCommandHandler(IIdentityDbContext context, PasswordHasher passwordHasher, TokenProvider tokenProvider) : IRequestHandler<LoginUserCommand, string>
{
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await context.Users.GetByEmailAsync(request.Email, cancellationToken);

        Guard.Against.NotFound(request.Email, user);

        bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);

        Guard.Against.NotFound(false, verified);

        string token = tokenProvider.Create(user);

        return token;
    }
}