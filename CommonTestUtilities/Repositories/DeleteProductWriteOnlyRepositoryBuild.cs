using Moq;
using ProductClientHub.Domain.Repositories.Product;

namespace CommonTestUtilities.Repositories;

public class DeleteProductWriteOnlyRepositoryBuild
{
    public static IDeleteProductWriteOnlyRepository Build(bool deleteResult = true)
    {
        var mock = new Mock<IDeleteProductWriteOnlyRepository>();
        mock.Setup(r => r.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(deleteResult);
        return mock.Object;
    }
}
