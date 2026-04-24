using ProductClientHub.Domain.Security.Tokens;
using ProductClientHub.Infrastructure.Security.Tokens.Acess.Generator;

namespace CommonTestUtilities.Token;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "ttttttttttttttttttttttttttttttttttttttttt");
}
