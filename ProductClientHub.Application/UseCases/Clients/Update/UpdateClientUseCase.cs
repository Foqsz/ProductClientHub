using Mapster;
using ProductClientHub.Application.UseCases.Clients.Update;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Services.loggedClient;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Users.Update;

public class UpdateClientUseCase : IUpdateClientUseCase
{
    private readonly IClientWriteOnlyRepository _clientWriteOnlyRepository;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedClient _loggedClient;

    public UpdateClientUseCase(IClientWriteOnlyRepository clientWriteOnlyRepository,
        IClientReadOnlyRepository clientReadOnlyRepository,
        IUnitOfWork unitOfWork,
        ILoggedClient loggedClient)
    {
        _clientWriteOnlyRepository = clientWriteOnlyRepository;
        _clientReadOnlyRepository = clientReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedClient = loggedClient;
    }

    public async Task<ResponseClientUpdatedJson> Execute(RequestShortClientJson request)
    {
        Validate(request);

        var userLogged = await _loggedClient.User();

        var client = await _clientReadOnlyRepository.GetById(userLogged.Id) ?? throw new NotFoundException(ResourceMessagesExceptions.CLIENT_NOCONTENT);

        var emailExist = await _clientReadOnlyRepository.EmailAlreadyExists(request.Email);

        if(emailExist is not null && emailExist.Id != userLogged.Id)
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
