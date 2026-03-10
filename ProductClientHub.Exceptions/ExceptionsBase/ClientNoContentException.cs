using System.Net;

namespace ProductClientHub.Exceptions.ExceptionsBase;

public class ClientNoContentException : ProductClientHubException
{
    public ClientNoContentException(string errorMessage) : base(errorMessage)
    {
    }

    public override List<string> GetErrors() => new List<string> { Message };

    public override HttpStatusCode GetHttpStatusCode() => HttpStatusCode.NoContent;
}
