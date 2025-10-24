using Domain.Entities;

namespace Domain.Common.Interfaces;

public interface IEmailVerificationLinkFactory
{
    string Create(EmailVerificationToken emailVerificationToken);
}
