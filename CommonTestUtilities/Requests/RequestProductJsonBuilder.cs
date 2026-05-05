using ProductClientHub.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestProductJsonBuilder
{
    public static RequestProductJson Build(string Name, string Brand, decimal Price)
    {
        return new RequestProductJson
        {
            Name = Name,
            Brand = Brand,
            Price = Price
        };
    }
}
