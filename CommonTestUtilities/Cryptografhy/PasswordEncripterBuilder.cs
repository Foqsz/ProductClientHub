using ProductClientHub.Domain.Security.Cryptography;
using ProductClientHub.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptografhy;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new BCryptNet();
}
