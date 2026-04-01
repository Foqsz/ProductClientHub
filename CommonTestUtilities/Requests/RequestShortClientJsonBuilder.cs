using ProductClientHub.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestShortClientJsonBuilder
{
    public static RequestShortClientJson Build(string name, string email)
    {
        return new RequestShortClientJson
        {
            Name = name,
            Email = email
        };
    }
}
