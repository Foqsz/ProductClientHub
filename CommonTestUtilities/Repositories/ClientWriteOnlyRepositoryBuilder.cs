using Moq;
using ProductClientHub.Domain.Repositories.Client;

namespace CommonTestUtilities.Repositories;

public class ClientWriteOnlyRepositoryBuilder
{
    public static IClientWriteOnlyRepository Build()
    {
        var mock = new Mock<IClientWriteOnlyRepository>();

        return mock.Object;
    }
}
