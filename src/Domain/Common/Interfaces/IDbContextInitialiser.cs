namespace Domain.Common.Interfaces;

public interface IDbContextInitialiser
{
    Task InitialiseAsync();
}