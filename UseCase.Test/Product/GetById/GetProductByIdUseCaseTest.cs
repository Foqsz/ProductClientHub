using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using ProductClientHub.Application.UseCases.GetById;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCase.Test.Product.GetById;

public class GetProductByIdUseCaseTest
{
    [Fact]
    public async Task GetProductByIdTest_Success()
    {
        (var client, _) = ClientBuilder.Build();
        var product = ProductBuilder.Build(client);

        var useCase = CreateUseCase(product, productNull: false);

        var result = await useCase.Execute(product.Id);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(product.Name);
        result.Brand.ShouldBe(product.Brand);
        result.Price.ShouldBe(product.Price);
    }

    [Fact]
    public async Task GetProductByIdTest_NotFound()
    {
        var useCase = CreateUseCase(null, productNull: true);

        var result = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(Guid.NewGuid()));

        result.GetHttpStatusCode().ShouldBe(System.Net.HttpStatusCode.NotFound);
        result.Message.ShouldBe(ResourceMessagesExceptions.PRODUCT_NOTFOUND);
    }

    private static GetProductByIdUseCase CreateUseCase(ProductClientHub.Domain.Entities.Product? product, bool productNull)
    {
        var repositoryReadOnly = new ProductsReadOnlyRepositoryBuild();

        if(productNull.IsFalse())
            repositoryReadOnly.GetById(product);

        return new GetProductByIdUseCase(repositoryReadOnly.Build());
    }
}
