using Bogus;
using ProductClientHub.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class ProductBuilder
{
    public static IList<Product> Collection(Client client, uint count = 2)
    {
        var list = new List<Product>();

        if (count == 0)
            count = 1;

        for (int i = 0; i < count; i++)
        {
            var fakeProduct = Build(client);
            fakeProduct.Id = Guid.NewGuid();

            list.Add(fakeProduct);
        }

        return list;
    }

    public static Product Build(Client client)
    {
        return new Faker<Product>()
            .RuleFor(r => r.Name, f => f.Commerce.ProductName())
            .RuleFor(r => r.Brand, f => f.Company.CompanyName())
            .RuleFor(r => r.Price, f => f.Random.Decimal(1, 100))
            .RuleFor(r => r.ClientId, _ => client.Id);
    }
}