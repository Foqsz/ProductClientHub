using System.Net;

namespace ProductClientHub.Exceptions.ExceptionsBase;

public class InvalidLoginException : ProductClientHubException
{
    public InvalidLoginException() : base(ResourceMessagesExceptions.LOGIN_INVALID)
    { }

    public override List<string> GetErrors() => new List<string> { Message };

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
}
