using Mapster;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Clients.GetAll;

public class GetAllClientsUseCase : IGetAllClientsUseCase
{
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;

    public GetAllClientsUseCase(IClientReadOnlyRepository clientReadOnlyRepository)
    {
        _clientReadOnlyRepository = clientReadOnlyRepository;
    }

    public async Task<ResponseAllClientsJson> Execute()
    {
        var clients = await _clientReadOnlyRepository.GetAll();

        if (clients.Any().IsFalse())
            throw new NoContentException(ResourceMessagesExceptions.CLIENT_NOCONTENT);
        
        var mapping = clients.Adapt<List<ResponseShortClientJson>>();

        return new ResponseAllClientsJson
        {
            Clients = mapping 
        };
    }
}
