using Application.EmailVerificationTokens.Factory;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Entities;
using Domain.Identity.PasswordHashers;
using FluentEmail.Core;
using Games.Games.Commands.UpdateGame;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Users.Users.Commands.CreateUser;

public record CreateUserCommand : IRequest<UserVm>
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
}

public class CreateUserCommandHandler(
    IIdentityDbContext context,
    PasswordHasher passwordHasher,
    IFluentEmail fluentEmail,
    EmailVerificationLinkFactory emailVerificationLinkFactory,
    IMapper mapper) : IRequestHandler<CreateUserCommand, UserVm>
{
    public async Task<UserVm> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken: cancellationToken))
        {
            throw new Exception("The email is already in use");
        }

        DateTime utcNow = DateTime.UtcNow;
        
        var user = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = passwordHasher.HashPassword(request.Password),
            CreatedOn = utcNow,
            UpdatedOn = utcNow
        };

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken: cancellationToken);

        //await VerifyEmailAsync(user, cancellationToken: cancellationToken); // needs a proper setup so commenting out for the time being

        var userDto = mapper.Map<UserDto>(user);
        
        return mapper.Map<UserVm>(userDto);
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
        };

        context.EmailVerificationTokens.Add(verificationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken: cancellationToken);
        }
        catch (DbUpdateException e)
            when (e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            throw new Exception("The email is already in use", e);
        }
        
        string verificationLink = emailVerificationLinkFactory.Create(verificationToken);

        await fluentEmail
            .To(user.Email)
            .Subject("Email verification for GamesApi")
            .Body($"To verify your email address <a href='{verificationLink}'>click here</a>", isHtml: true)
            .SendAsync();
    }
}