using Mapster;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.Application.Services.Mapping;

public static class MapConfigurations
{
    public static void Configure()
    {
        TypeAdapterConfig<RequestClientJson, Domain.Entities.Client>
            .NewConfig()
            .Ignore(c => c.Id);

        TypeAdapterConfig<RequestProductJson, Domain.Entities.Product>
            .NewConfig()
            .Ignore(p => p.Id);

        TypeAdapterConfig<ResponseProductsJson, Domain.Entities.Product>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Products.Select(p => p.Id).FirstOrDefault())
            .Map(dest => dest.Name, src => src.Products.Select(p => p.Name).FirstOrDefault())
            .Map(dest => dest.Price, src => src.Products.Select(p => p.Price).FirstOrDefault())
            .Map(dest => dest.Brand, src => src.Products.Select(p => p.Brand).FirstOrDefault())
            .Map(dest => dest.Client, src => src);

        TypeAdapterConfig<ResponseShortProductJson, Domain.Entities.Product>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.Brand, src => src.Brand)
            .Map(dest => dest.Client, src => src);
    }
}
