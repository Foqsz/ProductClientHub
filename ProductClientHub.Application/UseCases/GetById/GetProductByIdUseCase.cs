using Mapster;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.GetById;

public class GetProductByIdUseCase : IGetProductByIdUseCase
{
    private readonly IProductsReadOnlyRepository _productReadOnlyRepository;

    public GetProductByIdUseCase(IProductsReadOnlyRepository productReadOnlyRepository)
    {
        _productReadOnlyRepository = productReadOnlyRepository;
    }

    public async Task<ResponseShortProductJson> Execute(Guid productId)
    {
        var product = await _productReadOnlyRepository.GetById(productId);

        if (product is null)
            throw new NotFoundException(ResourceMessagesExceptions.PRODUCT_NOTFOUND);

        return product.Adapt<ResponseShortProductJson>();
    }
}
