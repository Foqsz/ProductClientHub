using Mapster;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Domain.Repositories.Client.Register;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.GetAll;

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
            throw new ClientNoContentException(ResourceMessagesExceptions.CLIENT_NOCONTENT);
        
        var mapping = clients.Adapt<List<ResponseClientJson>>();

        return new ResponseAllClientsJson
        {
            Clients = mapping 
        };
    }
}
