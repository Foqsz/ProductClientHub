using Mapster;
using ProductClientHub.Application.UseCases.Products.SharedValidator;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.Product;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Services.loggedClient;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Products.Register;

public class RegisterProductUseCase : IRegisterProductUseCase
{
    private readonly IProductsWriteOnlyRepository _productsWriteOnlyRepository;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedClient _loggedClient;

    public RegisterProductUseCase(IProductsWriteOnlyRepository productsWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IClientReadOnlyRepository clientReadOnlyRepository,
        ILoggedClient loggedClient)
    {
        _productsWriteOnlyRepository = productsWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _clientReadOnlyRepository = clientReadOnlyRepository;
        _loggedClient = loggedClient;
    }

    public async Task<ResponseShortProductJson> Execute(RequestProductJson request)
    {
        var user = await _loggedClient.User();

        await Validate(user.Id, request);

        var entity = request.Adapt<Domain.Entities.Product>();
        entity.ClientId = user.Id;

        await _productsWriteOnlyRepository.Add(entity);
        await _unitOfWork.Commit();

        return entity.Adapt<ResponseShortProductJson>();
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
