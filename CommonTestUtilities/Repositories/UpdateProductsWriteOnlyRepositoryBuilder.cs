using Moq;
using ProductClientHub.Domain.Repositories.Product;

namespace CommonTestUtilities.Repositories;

public class UpdateProductsWriteOnlyRepositoryBuilder
{
    public static IUpdateProductOnlyRepository Build()
    {
        var mock = new Mock<IUpdateProductOnlyRepository>();
        return mock.Object; 
    }
}
