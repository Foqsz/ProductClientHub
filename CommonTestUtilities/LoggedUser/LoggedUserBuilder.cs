using Moq;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Services.LoggedUser;

namespace CommonTestUtilities.LoggedUser;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(Client client)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(c => c.User()).ReturnsAsync(client);

        return mock.Object;
    }
}