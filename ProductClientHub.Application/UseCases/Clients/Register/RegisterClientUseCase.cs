using Mapster;
using ProductClientHub.Application.UseCases.Users.SharedValidator;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Domain.Security.Cryptography;
using ProductClientHub.Domain.Services.Messaging;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Users.Register;

public class RegisterClientUseCase : IRegisterClientUseCase
{
    private readonly IClientWriteOnlyRepository _clientWriteOnlyRepository;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IMessagePublisher _messagePublisher;

    public RegisterClientUseCase(IClientWriteOnlyRepository clientWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IClientReadOnlyRepository clientReadOnlyRepository,
        IPasswordEncripter passwordEncripter,
        IMessagePublisher messagePublisher)
    {
        _clientWriteOnlyRepository = clientWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _clientReadOnlyRepository = clientReadOnlyRepository;
        _passwordEncripter = passwordEncripter;
        _messagePublisher = messagePublisher;
    }

    public async Task<ResponseShortClientJson> Execute(RequestClientJson request)
    {
        Validate(request);

        var userExist = await _clientReadOnlyRepository.EmailAlreadyExists(request.Email);

        if (userExist is not null)
        {
            throw new EmailAlreadyExistsException(ResourceMessagesExceptions.EMAIL_INVALID);
        }

        var entity = request.Adapt<Client>();

        entity.Password = _passwordEncripter.Encrypt(request.Password);

        await _clientWriteOnlyRepository.Add(entity);
        await _unitOfWork.Commit();

        await _messagePublisher.PublishAsync("clients.created", new
        {
            entity.Id,
            entity.Name,
            entity.Email,
            entity.CreatedOn
        });

        return entity.Adapt<ResponseShortClientJson>();
    }

    private static void Validate(RequestClientJson request)
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
