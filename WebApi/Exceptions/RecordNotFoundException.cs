using System.Net;

namespace WebApi.Exceptions;

public class RecordNotFoundException : ServiceException
{
    public RecordNotFoundException(string message) : base(message)
    {
    }

    public override HttpStatusCode GetStatusCode()
    {
        return HttpStatusCode.NotFound;
    }
}
