using Moq;
using ProductClientHub.Domain.Repositories.Product;

namespace CommonTestUtilities.Repositories;

public class UpdateProductWriteOnlyRepository
{
    public static IUpdateProductOnlyRepository Build()
    {
        var mock = new Mock<IUpdateProductOnlyRepository>();

        return mock.Object;
    }
}
