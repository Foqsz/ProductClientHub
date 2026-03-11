using Mapster;
using ProductClientHub.Application.UseCases.Products.SharedValidator;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Products.Register;

public class RegisterProductUseCase : IRegisterProductUseCase
{
    private readonly IProductsWriteOnlyRepository _productsWriteOnlyRepository;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterProductUseCase(IProductsWriteOnlyRepository productsWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IClientReadOnlyRepository clientReadOnlyRepository)
    {
        _productsWriteOnlyRepository = productsWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _clientReadOnlyRepository = clientReadOnlyRepository;
    }

    public async Task<ResponseProductJson> Execute(Guid clientId, RequestProductJson request)
    {
        await Validate(clientId, request);

        var entity = request.Adapt<Domain.Entities.Product>();
        entity.Id = entity.Id;
        entity.ClientId = clientId;

        await _productsWriteOnlyRepository.Add(entity);
        await _unitOfWork.Commit();

        return new ResponseProductJson()
        {
            Id = entity.Id,
            Name = entity.Name,
            Brand = entity.Brand,
            Price = entity.Price
        };
    }

    private async Task Validate(Guid clientId, RequestProductJson request)
    {
        var clientExist = await _clientReadOnlyRepository.GetById(clientId);

        if(clientExist is null)
            throw new NotFoundException(ResourceMessagesExceptions.CLIENT_NOCONTENT);

        var validator = new RequestProductValidator();

        var validationResult = validator.Validate(request);

        if (validationResult.IsValid.IsFalse())
        {
            var errors = validationResult.Errors.Select(failure => failure.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
