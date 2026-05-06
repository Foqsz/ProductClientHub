using CommonTestUtilities.Entities;
using CommonTestUtilities.loggedClient;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using ProductClientHub.Application.UseCases.Products.Update;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCase.Test.Product.Update;

public class UpdateProductsUseCaseTest
{
    [Fact]
    public async Task UpdateProductUseCaseTest_Success()
    {
        (var client, _) = ClientBuilder.Build();

        var product = ProductBuilder.Build(client);

        var productUpdate = RequestProductJsonBuilder.Build(
            Name: "New Name",
            Brand: "New Brand", 
            Price: 100);

        var useCase = CreateUseCase(product, client, clientNull: false, productNull: false);

        var result = await useCase.Execute(product.Id, productUpdate);

        result.ShouldNotBeNull();
        result.Brand.ShouldBe(product.Brand);
        result.Name.ShouldBe(product.Name);
        result.Price.ShouldBe(product.Price);
    }

    [Fact]
    public async Task UpdateProductUseCaseTest_Client_NotFound()
    {
        (var client, _) = ClientBuilder.Build();

        var product = ProductBuilder.Build(client);

        var productUpdate = RequestProductJsonBuilder.Build(
            Name: "New Name",
            Brand: "New Brand", 
            Price: 100);

        var useCase = CreateUseCase(product, client, clientNull: true, productNull: false);

        var exception = await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(product.Id, productUpdate));

        exception.Message.ShouldBe(ResourceMessagesExceptions.CLIENT_NOCONTENT);
        exception.GetHttpStatusCode().ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProductUseCaseTest_Product_NotFound()
    {
        (var client, _) = ClientBuilder.Build();

        var product = ProductBuilder.Build(client);

        var productUpdate = RequestProductJsonBuilder.Build(
            Name: "New Name",
            Brand: "New Brand", 
            Price: 100);

        var useCase = CreateUseCase(product, client, clientNull: false, productNull: true);

        var exception = await Should.ThrowAsync<NotFoundException>(() => useCase.Execute(product.Id, productUpdate));

        exception.Message.ShouldBe(ResourceMessagesExceptions.PRODUCT_NOTFOUND);
        exception.GetHttpStatusCode().ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    private static UpdateProductUseCase CreateUseCase(ProductClientHub.Domain.Entities.Product product, ProductClientHub.Domain.Entities.Client client, bool clientNull, bool productNull)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var clientReadOnlyRepository = new ClientReadOnlyRepositoryBuilder();
        var productUpdateWriteOnlyRepository = UpdateProductsWriteOnlyRepositoryBuilder.Build();
        var productReadOnlyRepository = new ProductsReadOnlyRepositoryBuild();
        var loggedClient = LoggedClientBuilder.Build(client);

        if (clientNull.IsFalse())
        {
            clientReadOnlyRepository.GetById(client);
        }

        if (productNull.IsFalse())
        {
            productReadOnlyRepository.GetById(product);
        }
            
        
        return new UpdateProductUseCase(unitOfWork, productUpdateWriteOnlyRepository, clientReadOnlyRepository.Build(), productReadOnlyRepository.Build(), loggedClient);
    }
}
