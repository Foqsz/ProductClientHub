using ProductClientHub.Communication.Requests;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Security.Cryptography;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Clients.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IClientWriteOnlyRepository _clientWriteOnlyRepository;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordUseCase(IPasswordEncripter passwordEncripter,
        IClientWriteOnlyRepository clientWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IClientReadOnlyRepository clientReadOnlyRepository)
    {
        _passwordEncripter = passwordEncripter;
        _clientWriteOnlyRepository = clientWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _clientReadOnlyRepository = clientReadOnlyRepository;
    }

    public async Task Execute(Guid clientId, RequestChangePassword request)
    {
        Validate(request);

        var client = await _clientReadOnlyRepository.GetById(clientId);

        if(client is null)
            throw new NotFoundException(ResourceMessagesExceptions.CLIENT_NOCONTENT);

        var changeVerify = _passwordEncripter.IsValid(request.NewPassword, client.Password);

        if(changeVerify.IsTrue())
            throw new ChangePasswordException(ResourceMessagesExceptions.PASSWORD_INVALID);

        client.Password = _passwordEncripter.Encrypt(request.NewPassword);

        await _clientWriteOnlyRepository.Update(client);
        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePassword request)
    {
        var validator = new PasswordChangeValidator();

        var validationResult = validator.Validate(request);

        if (validationResult.IsValid.IsFalse())
        {
            var errors = validationResult.Errors.Select(failure => failure.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
