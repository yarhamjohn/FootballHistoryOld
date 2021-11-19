using Microsoft.Extensions.DependencyInjection;

namespace football.history.api.Tests;

public static class ServiceCollectionExtensions
{
    public static void SwapTransient<TService>(
        this IServiceCollection serviceCollection,
        TService mockImplementation)
    {
        var serviceDescriptors = serviceCollection
            .Where(descriptor =>
                descriptor.ServiceType == typeof(TService)
                && descriptor.Lifetime == ServiceLifetime.Transient)
            .ToArray();

        foreach (var serviceDescriptor in serviceDescriptors)
        {
            serviceCollection.Remove(serviceDescriptor);
        }

        serviceCollection.AddTransient(typeof(TService),_ => mockImplementation!);
    }
}