using CommonTestUtilities.Entities;
using CommonTestUtilities.loggedClient;
using CommonTestUtilities.Repositories;
using ProductClientHub.Application.UseCases.Products.Delete;
using ProductClientHub.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCase.Test.Product.Delete;

public class DeleteProductUseCaseTest
{
    [Fact]
    public async Task DeleteProductUseCaseTest_Success()
    {
        (var client, _) = ClientBuilder.Build();

        var product = ProductBuilder.Build(client);

        var useCase = CreateUseCase(product, client);

        var result = useCase.Execute(product.Id);

        await result.ShouldNotBeNull();
        result.IsCompleted.ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteProductUseCaseTest_NotFoundProduct()
    {
        (var client, _) = ClientBuilder.Build();

        var product = ProductBuilder.Build(client);

        var useCase = CreateUseCase(product, client, false);

        var result = await Should.ThrowAsync<NoContentException>(async () => await useCase.Execute(Guid.NewGuid()));
         
        result.Message.ShouldBe(ResourceMessagesExceptions.PRODUCT_INVALID);
        result.GetHttpStatusCode().ShouldBe(System.Net.HttpStatusCode.NoContent);
    }

    private static DeleteProductUseCase CreateUseCase(ProductClientHub.Domain.Entities.Product product, ProductClientHub.Domain.Entities.Client client, bool deleteResult = true)
    {
        var deleteProduct = DeleteProductWriteOnlyRepositoryBuild.Build(deleteResult);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var clientLogged = LoggedClientBuilder.Build(client);

        return new DeleteProductUseCase(deleteProduct, unitOfWork, clientLogged);   
    }
}
