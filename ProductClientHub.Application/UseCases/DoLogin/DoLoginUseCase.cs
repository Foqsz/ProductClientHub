using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Security.Cryptography;
using ProductClientHub.Domain.Security.Tokens;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IPasswordEncripter _passwordEncripter;

    public DoLoginUseCase(IAccessTokenGenerator accessTokenGenerator, 
        IClientReadOnlyRepository clientReadOnlyRepository,
        IPasswordEncripter passwordEncripter)
    {
        _accessTokenGenerator = accessTokenGenerator;
        _clientReadOnlyRepository = clientReadOnlyRepository;
        _passwordEncripter = passwordEncripter;
    }


    public async Task<ResponseTokenJson> Execute(RequestLoginJson request)
    {
        var userExist = await _clientReadOnlyRepository.EmailAlreadyExists(request.Email);

        if (userExist is null || _passwordEncripter.IsValid(request.Password, userExist.Password).IsFalse())
            throw new InvalidLoginException();

        var token = _accessTokenGenerator.Generate(userIdentifier: userExist.Id);

        return new ResponseTokenJson()
        {
            Token = token
        };
    }
}
