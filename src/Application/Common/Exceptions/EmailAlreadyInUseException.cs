using System.Runtime.Serialization;

namespace Application.Common.Exceptions;

[Serializable]
public class EmailAlreadyInUseException : Exception
{
    public EmailAlreadyInUseException() : base("The email is already in use.")
    {
    }

    public EmailAlreadyInUseException(string email) : base($"The email '{email}' is already in use.")
    {
    }

    public EmailAlreadyInUseException(string email, Exception innerException) : base($"The email '{email}' is already in use.", innerException)
    {
    }

}