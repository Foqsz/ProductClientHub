using Microsoft.Extensions.DependencyInjection;

namespace ProductClientHub.Application;

public static class DependencyInjectionExtension
{
    public static void AddAplication(this IServiceCollection services)
    {
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {

    }
}
