using System.Net;

namespace WebApi.Exceptions;

public class ValidationException : ServiceException
{
    public ValidationException(string message) : base(message)
    {
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.BadRequest;
    }
}
