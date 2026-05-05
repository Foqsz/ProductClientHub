using CommonTestUtilities.Entities;
using CommonTestUtilities.loggedClient;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using ProductClientHub.Application.UseCases.Products.Register;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCase.Test.Product.Register;

public class RegisterProductUseCaseTest
{
    [Fact]
    public async Task RegisterProductUseCaseTest_Success()
    {
        (var client, _) = ClientBuilder.Build();
        var product = ProductBuilder.Build(client);

        var productJson =RequestProductJsonBuilder.Build(product.Name, product.Brand, product.Price);

        var useCase = CreateUseCase(product, client, ClientNull: false);
         
        var result = await useCase.Execute(productJson);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(product.Name);
        result.Brand.ShouldBe(product.Brand);
        result.Price.ShouldBe(product.Price);
    }

    [Fact]
    public async Task RegisterProductUseCaseTest_Client_NotFound_Error()
    {
        (var client, _) = ClientBuilder.Build();
        var product = ProductBuilder.Build(client);

        var productJson = RequestProductJsonBuilder.Build(product.Name, product.Brand, product.Price);

        var useCase = CreateUseCase(product, client, ClientNull: true);

        var result = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(productJson));

        result.Message.ShouldBe(ResourceMessagesExceptions.CLIENT_NOCONTENT);
        result.GetHttpStatusCode().ShouldBe(System.Net.HttpStatusCode.NotFound);
    }

    private static RegisterProductUseCase CreateUseCase(ProductClientHub.Domain.Entities.Product product, ProductClientHub.Domain.Entities.Client client, bool ClientNull)
    {
        var productsWriteOnly = ProductsWriteOnlyRepositoryBuild.Build();
        var clientReadOnly = new ClientReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedClient = LoggedClientBuilder.Build(client);

        if(ClientNull.IsFalse())
            clientReadOnly.GetById(client);

        return new RegisterProductUseCase(productsWriteOnly, unitOfWork, clientReadOnly.Build(), loggedClient);
    }
}
