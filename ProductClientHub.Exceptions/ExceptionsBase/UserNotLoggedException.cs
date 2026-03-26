using System.Net;

namespace ProductClientHub.Exceptions.ExceptionsBase;

public class UserNotLoggedException : ProductClientHubException
{
    public UserNotLoggedException(string errorMessage) : base(errorMessage)
    {
    }

    public override List<string> GetErrors() => new List<string> { Message };

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.Unauthorized;
}
