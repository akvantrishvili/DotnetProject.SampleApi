
using System.Reflection;
using DotnetProject.SampleApi.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetProject.SampleApi.Application
{
    /// <summary>
    ///     Dependency Resolver for Application Layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        ///     Register all necessary types of application layer.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns>Service collection with registered necessary types</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services, Assembly? assembly = null)
        {
            services.AddMediatR(t =>
            {
                t.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
                t.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            return services;
        }
    }
}
