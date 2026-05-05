using CommonTestUtilities.loggedClient;
using CommonTestUtilities.Repositories;
using ProductClientHub.Application.UseCases.Products.Update;

namespace UseCase.Test.Product.Update;

public class UpdateProductsUseCaseTest
{
    private static UpdateProductUseCase CreateUseCase(ProductClientHub.Domain.Entities.Product product, ProductClientHub.Domain.Entities.Client client, bool ClientNull)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var clientReadOnlyRepository = new ClientReadOnlyRepositoryBuilder();
        var productReadOnlyRepository = new ProductsReadOnlyRepositoryBuild();
        var loggedClient = LoggedClientBuilder.Build(client);
    }
}
