using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace Domain.Entities;

public class EmailVerificationToken : BaseAuditableEntity
{
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public DateTime ExpiresOn { get; set; }

    public User User { get; set; }
}