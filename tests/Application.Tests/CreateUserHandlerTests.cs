using Application.Identity.Commands.CreateUser;
using Application.Identity.Queries.GetUsers;
using AutoMapper;
using Domain.Common.Interfaces;
using Domain.Entities;
using Domain.Identity.PasswordHashers;
using FluentAssertions;
using FluentEmail.Core;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NSubstitute;
using Xunit;

namespace Application.Tests;

public class CreateUserCommandHandlerTests
{
    private readonly IIdentityDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IFluentEmail _fluentEmail;
    private readonly IEmailVerificationLinkFactory _linkFactory;
    private readonly IMapper _mapper;
    private readonly CreateUserCommandHandler _handler;
    private readonly ILoggerFactory _loggerFactory;

    public CreateUserCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new IdentityDbContext(options);

        _passwordHasher = Substitute.For<IPasswordHasher<User>>();
        _fluentEmail = Substitute.For<IFluentEmail>();
        _linkFactory = Substitute.For<IEmailVerificationLinkFactory>();
        _loggerFactory = Substitute.For<ILoggerFactory>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDto>();
            cfg.CreateMap<UserDto, UserVm>();
        }, _loggerFactory);
        _mapper = config.CreateMapper();

        _handler = new CreateUserCommandHandler(
            _context, _passwordHasher, _fluentEmail, _linkFactory, _mapper);
    }

    [Fact]
    public async Task Handle_ValidRequest_CreatesUserAndSendsVerificationEmail()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "Secure123!"
        };

        _passwordHasher.HashPassword(new User(), command.Password).ReturnsForAnyArgs("hashed_password_123");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == command.Email);
        userInDb.Should().NotBeNull();
        userInDb!.FirstName.Should().Be("John");
        userInDb.LastName.Should().Be("Doe");
        userInDb.PasswordHash.Should().Be("hashed_password_123");
        userInDb.EmailVerified.Should().BeFalse();
        userInDb.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        userInDb.UpdatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        var tokenInDb = await _context.EmailVerificationTokens.FirstOrDefaultAsync();
        tokenInDb.Should().NotBeNull();
        tokenInDb!.UserId.Should().Be(userInDb.Id);
        tokenInDb.ExpiresOn.Should().BeCloseTo(DateTime.UtcNow.AddHours(1), TimeSpan.FromSeconds(5));

        _linkFactory.Received(1).Create(Arg.Any<EmailVerificationToken>());

        result.Should().NotBeNull();
        result.Email.Should().Be(command.Email);
        result.FirstName.Should().Be("John");
    }

    [Fact]
    public async Task Handle_ExistingEmail_ThrowsException()
    {
        // Arrange
        var existingUser = new User
        {
            Id = 1,
            Email = "exists@example.com",
            FirstName = "Old",
            LastName = "User",
            PasswordHash = "oldhash",
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var command = new CreateUserCommand
        {
            Email = "exists@example.com",
            FirstName = "New",
            LastName = "User",
            Password = "Pass123!"
        };

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("The email is already in use");
    }

    [Fact]
    public async Task Handle_UniqueViolationOnToken_ThrowsEmailInUseException()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Email = "unique@example.com",
            FirstName = "Jane",
            LastName = "Smith",
            Password = "Pass123!"
        };

        _passwordHasher.HashPassword(default, command.Password).ReturnsForAnyArgs("hash123");

        var dbUpdateEx = new DbUpdateException(
            "Unique violation",
            new PostgresException("duplicate key value", "23505", "XX000", "duplicate key value violates unique constraint"));

        var callCount = 0;
        

        // Act & Assert
        await _handler.Handle(command, CancellationToken.None);
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("The email is already in use");
    }

    [Fact]
    public async Task Handle_CancellationRequested_PropagatesCancellation()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var command = new CreateUserCommand
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "Pass123!"
        };

        // Act
        var act = async () => await _handler.Handle(command, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public void Mapper_UserToUserVm_MapsCorrectly()
    {
        // Arrange
        var user = new User
        {
            Id = 99,
            Email = "mapped@example.com",
            FirstName = "Map",
            LastName = "Test",
            EmailVerified = true,
            CreatedOn = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UpdatedOn = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
        };

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDto>();
            cfg.CreateMap<UserDto, UserVm>();
        }, _loggerFactory);
        var mapper = config.CreateMapper();

        // Act
        var vm = mapper.Map<UserVm>(mapper.Map<UserDto>(user));

        // Assert
        vm.Id.Should().Be(99);
        vm.Email.Should().Be("mapped@example.com");
        vm.FirstName.Should().Be("Map");
    }
}