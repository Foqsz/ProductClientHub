using Microsoft.EntityFrameworkCore;
using ProductClientHub.Domain.Entities;
using ProductClientHub.Domain.Security.Tokens;
using ProductClientHub.Domain.Services.LoggedUser;
using ProductClientHub.Infrastructure.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductClientHub.Infrastructure.Services;

public class LoggedUser : ILoggedUser
{
    private readonly ProductClientHubDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(ProductClientHubDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<Client> User()
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(identifier);

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.Id == userIdentifier);
    }
}
