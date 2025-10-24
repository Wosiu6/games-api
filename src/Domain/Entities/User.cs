using Domain.Common;

namespace Domain.Entities;

public class User : BaseAuditableEntity
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool EmailVerified { get; set; }
}