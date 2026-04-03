using Moq;
using ProductClientHub.Domain.Repositories.Client;

namespace CommonTestUtilities.Repositories;

public class ClientDeleteAccountOnlyRepositoryBuilder
{
    public static IDeleteClientRepository Build()
    {
        var mock = new Mock<IDeleteClientRepository>();
        return mock.Object;
    }
}
