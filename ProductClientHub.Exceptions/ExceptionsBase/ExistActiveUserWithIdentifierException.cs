using System.Net;

namespace ProductClientHub.Exceptions.ExceptionsBase;

public class ExistActiveUserWithIdentifierException : ProductClientHubException
{
    public ExistActiveUserWithIdentifierException(string message) : base(message)
    {
    }
    public override List<string> GetErrors() => new List<string> { Message };

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
}
