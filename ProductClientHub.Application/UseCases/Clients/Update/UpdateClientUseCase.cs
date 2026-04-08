using Mapster;
using ProductClientHub.Application.UseCases.Clients.Update;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Users.Update;

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

    public async Task<ResponseClientUpdatedJson> Execute(Guid clientId, RequestShortClientJson request)
    {
        Validate(request);

        var client = await _clientReadOnlyRepository.GetById(clientId) ?? throw new NotFoundException(ResourceMessagesExceptions.CLIENT_NOCONTENT);

        var emailExist = await _clientReadOnlyRepository.EmailAlreadyExists(request.Email);

        if(emailExist is not null)
            throw new EmailAlreadyExistsException(ResourceMessagesExceptions.EMAIL_INVALID);

        client.Name = request.Name;
        client.Email = request.Email;

        await _clientWriteOnlyRepository.Update(client);
        await _unitOfWork.Commit();

        return client.Adapt<ResponseClientUpdatedJson>();
    }

    private static void Validate(RequestShortClientJson request)
    {
        var validator = new UpdateClientValidator();

        var validationResult = validator.Validate(request);

        if (validationResult.IsValid.IsFalse())
        {
            var errors = validationResult.Errors.Select(failure => failure.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
