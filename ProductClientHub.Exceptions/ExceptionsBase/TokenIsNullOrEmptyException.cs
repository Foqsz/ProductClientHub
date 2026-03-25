using System.Net;

namespace ProductClientHub.Exceptions.ExceptionsBase;

public class TokenIsNullOrEmptyException : ProductClientHubException
{
    public TokenIsNullOrEmptyException(string message) : base(message)
    {
    }
    public override List<string> GetErrors() => new List<string> { Message };

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
}
