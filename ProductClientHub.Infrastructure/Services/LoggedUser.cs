using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Security.Tokens;
using ProductClientHub.Domain.Services.loggedClient;
using ProductClientHub.Infrastructure.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductClientHub.Infrastructure.Services;

public class loggedClient : ILoggedClient
{
    private readonly ProductClientHubDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public loggedClient(ProductClientHubDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<Client> User()
    {
        var token = _tokenProvider.Value(); // Obtém o token JWT do provedor de tokens, que é usado para identificar o usuário logado.

        var tokenHandler = new JwtSecurityTokenHandler(); // Cria um manipulador de tokens JWT para ler o token e extrair as informações necessárias.   

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token); // Lê o token JWT e o converte em um objeto JwtSecurityToken, que contém as reivindicações (claims) do token, incluindo o identificador do usuário.

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value; // Extrai o valor da reivindicação (claim) do tipo ClaimTypes.Sid, que é usada para armazenar o identificador do usuário. O valor é convertido para uma string.

        var userIdentifier = Guid.Parse(identifier); // Converte o identificador do usuário, que é uma string, para um objeto Guid, que é o tipo de dados usado para armazenar o identificador do usuário no banco de dados.

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.Id == userIdentifier);
    }
}
