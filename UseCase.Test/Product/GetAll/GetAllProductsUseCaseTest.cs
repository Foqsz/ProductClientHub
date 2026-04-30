using CommonTestUtilities.Entities;
using CommonTestUtilities.loggedClient;
using CommonTestUtilities.Repositories;
using ProductClientHub.Application.UseCases.Products.GetAll;
using ProductClientHub.Domain.Extensions;
using Shouldly;

namespace UseCase.Test.Product.GetAll;

public class GetAllProductsUseCaseTest
{
    [Fact]
    public async Task GetAllProductsTest_Success()
    {
        (var client, _) = ClientBuilder.Build();

        var useCase = CreateUseCase(client, clientIsNull: false);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Products.ShouldNotBeNull();
        result.Products.Count.ShouldBe(client.Products.Count);
    }
    private static GetAllProductsUseCase CreateUseCase(ProductClientHub.Domain.Entities.Client client, bool clientIsNull)
    {
        var repositoryReadOnly = new ProductsReadOnlyRepositoryBuild();
        var userLogged = LoggedClientBuilder.Build(client!);

        if(client is not null && clientIsNull.IsFalse())
        {
            var products = ProductBuilder.Collection(client);
            repositoryReadOnly.GetAll(products);
        }

        return new GetAllProductsUseCase(repositoryReadOnly.Build(), userLogged);
    }
}
