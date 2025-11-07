using Domain.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using Application.Common.Exceptions;

namespace Application.Identity.Commands.CreateUser;

public record CreateUserCommand : IRequest<int>
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class CreateUserCommandHandler(
    IIdentityDbContext context,
    IPasswordHasher<User> passwordHasher,
    IFluentEmail fluentEmail,
    IEmailVerificationLinkFactory emailVerificationLinkFactory) : IRequestHandler<CreateUserCommand, int>
{
    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken: cancellationToken))
        {
            throw new EmailAlreadyInUseException(request.Email);
        }

        DateTime utcNow = DateTime.UtcNow;

        var user = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedOn = utcNow,
            UpdatedOn = utcNow,
            EmailVerified = false // will be set after verification
        };
        
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken: cancellationToken);

        // create verification token and send email
        await VerifyEmailAsync(user, cancellationToken);

        return user.Id;
    }

    private async Task VerifyEmailAsync(User user, CancellationToken cancellationToken)
    {
        DateTime utcNow = DateTime.UtcNow;
        var verificationToken = new EmailVerificationToken
        {
            UserId = user.Id,
            CreatedOn = utcNow,
            UpdatedOn = utcNow,
            ExpiresOn = utcNow.AddHours(1),
            User = user
        };

        context.EmailVerificationTokens.Add(verificationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken: cancellationToken);
        }
        catch (DbUpdateException e)
            when (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new EmailAlreadyInUseException(user.Email, e);
        }

        string verificationLink = emailVerificationLinkFactory.Create(verificationToken);

        await fluentEmail
            .To(user.Email)
            .Subject("Email verification for Scriptorium API")
            .Body($"To verify your email address <a href='{verificationLink}'>click here</a>", isHtml: true)
            .SendAsync();
    }
}
