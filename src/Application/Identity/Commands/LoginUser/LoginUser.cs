using Ardalis.GuardClauses;
using Domain.Common.Data.Extensions;
using Domain.Common.Interfaces;
using Domain.Entities;
using Domain.Identity.IdentityProviders;
using Domain.Identity.PasswordHashers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Identity.Commands.LoginUser;

public record LoginUserCommand : IRequest<string>
{
    [Required]
    [EmailAddress]
    [DefaultValue("admin@example.com")]
    public string Email { get; set; } = null!;

    [Required]
    [DefaultValue("AdminPassword123£")]
    public string Password { get; set; } = null!;
}

public class LoginUserCommandHandler(IIdentityDbContext context, IPasswordHasher<User> passwordHasher, TokenProvider tokenProvider) : IRequestHandler<LoginUserCommand, string>
{
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await context.Users.GetByEmailAsync(request.Email, cancellationToken);

        Guard.Against.NotFound(request.Email, user);

        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        var isVerified = verificationResult == PasswordVerificationResult.Success
            || verificationResult == PasswordVerificationResult.SuccessRehashNeeded;

        Guard.Against.NotFound(false, isVerified);

        string token = tokenProvider.Create(user);

        return token;
    }
}
