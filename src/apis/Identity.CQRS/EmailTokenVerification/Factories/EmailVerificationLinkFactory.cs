using Domain.EndpointConstants;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.EmailTokenVerification.Factories;

public sealed class EmailVerificationLinkFactory(
    IHttpContextAccessor httpContextAccessor,
    LinkGenerator linkGenerator)
{
    public string Create(EmailVerificationToken emailVerificationToken)
    {
        string? verificationLink = linkGenerator.GetUriByName(
            httpContextAccessor.HttpContext!,
            EndpointsNames.VerifyEmail,
            new { token = emailVerificationToken.Id });

        return verificationLink ?? throw new Exception("Could not create email verification link");
    }
}