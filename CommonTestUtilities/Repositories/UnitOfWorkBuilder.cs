using ProductClientHub.Domain.Repositories.UnitOfWork;

namespace CommonTestUtilities.Repositories;

public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Moq.Mock<IUnitOfWork>();
        return mock.Object;
    }
}
