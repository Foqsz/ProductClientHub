using System.Net;

namespace ProductClientHub.Exceptions.ExceptionsBase;

public class EmailAlreadyExistsException : ProductClientHubException
{
    public EmailAlreadyExistsException(string errorMessage) : base(errorMessage)
    {
    }

    public override List<string> GetErrors() => new List<string> { Message };

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.BadRequest;
}
