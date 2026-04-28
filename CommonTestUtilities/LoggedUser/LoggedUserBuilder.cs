using Moq;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Services.loggedClient;

namespace CommonTestUtilities.loggedClient;

public class LoggedClientBuilder
{
    public static ILoggedClient Build(Client client)
    {
        var mock = new Mock<ILoggedClient>();

        mock.Setup(c => c.User()).ReturnsAsync(client);

        return mock.Object;
    }
}