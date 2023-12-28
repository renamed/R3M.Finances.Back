using System.Net;

namespace WebApi.Exceptions;

public abstract class ServiceException : Exception
{
    public ServiceException(string message) : base(message)
    {
    }

    public abstract HttpStatusCode GetStatusCode();
}
