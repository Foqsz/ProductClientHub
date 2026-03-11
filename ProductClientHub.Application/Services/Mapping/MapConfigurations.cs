using Mapster;
using ProductClientHub.Communication.Requests;

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
    }
}
