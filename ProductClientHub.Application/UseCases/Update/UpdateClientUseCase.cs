using ProductClientHub.Application.UseCases.Clients.SharedValidator;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Update;

public class UpdateClientUseCase : IUpdateClientUseCase
{
    private readonly IClientWriteOnlyRepository _clientWriteOnlyRepository;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientUseCase(IClientWriteOnlyRepository clientWriteOnlyRepository,
        IClientReadOnlyRepository clientReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _clientWriteOnlyRepository = clientWriteOnlyRepository;
        _clientReadOnlyRepository = clientReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseClientUpdatedJson> Execute(Guid clientId, RequestClientJson request)
    {
        Validate(request);

        var client = await _clientReadOnlyRepository.GetById(clientId);

        if (client is null)
            throw new NotFoundException(ResourceMessagesExceptions.CLIENT_NOCONTENT);

        client.Name = request.Name;
        client.Email = request.Email;

        await _clientWriteOnlyRepository.Update(client);
        await _unitOfWork.Commit();

        return new ResponseClientUpdatedJson()
        {
            Name = request.Name,
            Email = request.Email
        };
    }

    private void Validate(RequestClientJson request)
    {
        var validator = new RequestClientValidator();

        var validationResult = validator.Validate(request);

        if (validationResult.IsValid.IsFalse())
        {
            var errors = validationResult.Errors.Select(failure => failure.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
