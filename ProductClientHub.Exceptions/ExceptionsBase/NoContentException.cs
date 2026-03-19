using System.Net;

namespace ProductClientHub.Exceptions.ExceptionsBase;

public class NoContentException : ProductClientHubException
{
    public NoContentException(string errorMessage) : base(errorMessage)
    {
    }

    public override List<string> GetErrors() => new List<string> { Message };

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.NoContent;
}
