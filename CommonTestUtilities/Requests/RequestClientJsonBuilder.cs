using ProductClientHub.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestClientJsonBuilder
{
    public static RequestClientJson Build(string name, string email, string password)
    {
        return new RequestClientJson
        {
            Name = name,
            Email = email,
            Password = password
        };
    }
}
