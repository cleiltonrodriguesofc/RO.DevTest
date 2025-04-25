using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


using Microsoft.Extensions.Configuration;

// Registers persistence-layer services like DbContext and repositories.
using RO.DevTest.Application.Interfaces.Repositories;
using RO.DevTest.Persistence.Repositories;



namespace RO.DevTest.Persistence.IoC;

    /// <summary>
    /// Inject the dependencies of the Persistence layer into an
    /// <see cref="IServiceCollection"/>
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to inject the dependencies into
    /// </param>
    /// <returns>
    /// The <see cref="IServiceCollection"/> with dependencies injected
    /// </returns>

public static class PersistenceDependencyInjector {
    public static IServiceCollection InjectPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // get database connection string from configuration
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        // register the application db context with postgresql provider
        services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(connectionString));
        
        // register repository interface with its concrete implementation for customer
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        // register repository interface with its concrete implementation for product
        services.AddScoped<IProductRepository, ProductRepository>();
        

        return services;
    }
}
