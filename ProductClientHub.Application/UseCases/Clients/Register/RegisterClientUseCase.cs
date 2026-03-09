using Mapster;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client.Register;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Clients.Register;

public class RegisterClientUseCase : IRegisterClientUseCase
{
    private readonly IClientWriteOnlyRepository _clientWriteOnlyRepository;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterClientUseCase(IClientWriteOnlyRepository clientWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IClientReadOnlyRepository clientReadOnlyRepository)
    {
        _clientWriteOnlyRepository = clientWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _clientReadOnlyRepository = clientReadOnlyRepository;
    }

    public async Task<ResponseClientJson> Execute(RequestClientJson request)
    {
        Validate(request);

        var userExist = await _clientReadOnlyRepository.EmailAlreadyExists(request.Email);

        if (userExist.IsTrue())
        {
            throw new EmailAlreadyExistsException(ResourceMessagesExceptions.EMAIL_INVALID);
        }

        var entity = request.Adapt<Client>();
        await _clientWriteOnlyRepository.Add(entity);
        await _unitOfWork.Commit();

        return new ResponseClientJson
        {
            Id = entity.Id,
            Name = request.Name
        };
    }

    private void Validate(RequestClientJson request)
    {
        var validator = new RegisterClientValidator();

        var validationResult = validator.Validate(request);

        if (validationResult.IsValid.IsFalse())
        {
            var errors = validationResult.Errors.Select(failure => failure.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
