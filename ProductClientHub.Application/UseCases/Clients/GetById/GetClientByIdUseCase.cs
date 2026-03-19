using Mapster;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Users.GetById;

public class GetClientByIdUseCase : IGetClientByIdUseCase
{
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;

    public GetClientByIdUseCase(IClientReadOnlyRepository clientReadOnlyRepository)
    {
        _clientReadOnlyRepository = clientReadOnlyRepository;
    }

    public async Task<ResponseClientJson> Execute(Guid clientId)
    {
        var clientExist = await _clientReadOnlyRepository.GetById(clientId);

        if (clientExist is null)
            throw new NotFoundException(ResourceMessagesExceptions.CLIENT_NOCONTENT);

        return clientExist.Adapt<ResponseClientJson>();
    }
}
