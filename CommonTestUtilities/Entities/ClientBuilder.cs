using Bogus;
using CommonTestUtilities.Cryptografhy;
using ProductClientHub.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class ClientBuilder
{
    public static (Client client, string password) Build()
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();

        var password = new Faker().Internet.Password();

        var client = new Faker<Client>()
            .RuleFor(client => client.Id, () => Guid.NewGuid())
            .RuleFor(client => client.Name, (f) => f.Person.FirstName)
            .RuleFor(client => client.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(client => client.Password, (f) => passwordEncripter.Encrypt(password))
            .RuleFor(client => client.Products, () => new List<Product>());

        return (client, password);
    }
}
