using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using vNext.Core.Interfaces;

namespace vNext.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProcedures(this IServiceCollection services, Assembly assembly)
        {            
            services.Scan(
                scan => scan.FromAssemblies(assembly)
                    .AddClasses(x => x.AssignableTo(typeof(IProcedure<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );

            return services;
        }
    }
}
