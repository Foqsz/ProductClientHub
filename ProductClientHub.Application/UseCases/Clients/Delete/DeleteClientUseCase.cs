using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Domain.Repositories.UnitOfWork;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Clients.Delete;

public class DeleteClientUseCase : IDeleteClientUseCase
{
    private readonly IDeleteClientRepository _deleteClientRepository;
    private readonly IClientReadOnlyRepository _clientReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientUseCase(IDeleteClientRepository deleteClientRepository,
        IUnitOfWork unitOfWork,
        IClientReadOnlyRepository clientReadOnlyRepository)
    {
        _deleteClientRepository = deleteClientRepository;
        _unitOfWork = unitOfWork;
        _clientReadOnlyRepository = clientReadOnlyRepository;
    }

    public async Task Execute(Guid clientId)
    {
        var client = await _clientReadOnlyRepository.GetById(clientId);

        if (client is null)
            throw new NotFoundException(ResourceMessagesExceptions.CLIENT_NOCONTENT);

        await _deleteClientRepository.Delete(client.Id);
        await _unitOfWork.Commit();
    }
}
