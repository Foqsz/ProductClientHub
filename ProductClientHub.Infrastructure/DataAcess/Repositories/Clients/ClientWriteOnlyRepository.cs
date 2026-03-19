using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Repositories.Client;
using ProductClientHub.Infrastructure.Database;

namespace ProductClientHub.Infrastructure.DataAcess.Repositories.Users;

public class ClientWriteOnlyRepository : IClientWriteOnlyRepository, IDeleteClientRepository
{
    private readonly ProductClientHubDbContext _dbContext;

    public ClientWriteOnlyRepository(ProductClientHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Client client)
    {
        await _dbContext.Users.AddAsync(client);
    }

    public async Task Delete(Guid clientId)
    {
        var client = await _dbContext.Users.Where(client => client.Id == clientId).FirstOrDefaultAsync();
        _dbContext.Users.Remove(client!);
    }

    public async Task<Client?> Update(Client client)
    {
        var createdOn = await _dbContext.Users
            .Where(x => x.Id == client.Id)
            .Select(x => x.CreatedOn)
            .FirstOrDefaultAsync();

        // Isso aqui foi necessário para evitar que o EF Core tente atualizar a coluna CreatedOn,
        // que é gerada automaticamente pelo banco de dados e não deve ser modificada.
        // Ao definir o valor de CreatedOn para o valor original do banco de dados,
        // garantimos que ele permaneça inalterado durante a atualização do cliente.
        client.CreatedOn = DateTime.SpecifyKind(createdOn, DateTimeKind.Utc);

        _dbContext.Users.Update(client);

        // Garantir que a propriedade CreatedOn não seja marcada como modificada, para que o EF Core não tente atualizá-la no banco de dados.
        _dbContext.Entry(client).Property(x => x.CreatedOn).IsModified = false;

        return client;
    }
}
