using Moq;
using ProductClientHub.Domain.Repositories.Product;

namespace CommonTestUtilities.Repositories;

public class ProductsReadOnlyRepositoryBuild
{
    private readonly Mock<IProductsReadOnlyRepository> _repository;

    public ProductsReadOnlyRepositoryBuild()
    {
        _repository = new Mock<IProductsReadOnlyRepository>();
    }

    public ProductsReadOnlyRepositoryBuild GetAll(IList<ProductClientHub.Domain.Entities.Product> products)
    {
        _repository.Setup(r => r.GetAll()).ReturnsAsync(products);
        return this;
    }

    public ProductsReadOnlyRepositoryBuild GetById(ProductClientHub.Domain.Entities.Product? product)
    {
        if(product is not null)
            _repository.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(product);
       
        return this;
    }

    public IProductsReadOnlyRepository Build()
    {
        return _repository.Object;
    }
}
