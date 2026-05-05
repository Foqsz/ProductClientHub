using Moq;
using ProductClientHub.Domain.Repositories.Product;

namespace CommonTestUtilities.Repositories;

public class ProductsWriteOnlyRepositoryBuild
{
    public static IProductsWriteOnlyRepository Build()
    {
        var mock = new Mock<IProductsWriteOnlyRepository>();
        return mock.Object;
    }
}
