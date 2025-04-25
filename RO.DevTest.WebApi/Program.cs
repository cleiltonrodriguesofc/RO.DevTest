using RO.DevTest.Application;
using RO.DevTest.Infrastructure.IoC;
using RO.DevTest.Persistence.IoC;

using Microsoft.EntityFrameworkCore;
using RO.DevTest.Persistence;

namespace RO.DevTest.WebApi;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.InjectPersistenceDependencies(builder.Configuration)
            .InjectInfrastructureDependencies();

        // register customer use case handlers
        // create
        builder.Services.AddScoped<
            RO.DevTest.Application.Interfaces.UseCases.Customer.ICreateCustomerHandler,
            RO.DevTest.Application.UseCases.Customer.Create.CreateCustomerHandler>();

        // update
        builder.Services.AddScoped<
            RO.DevTest.Application.Interfaces.UseCases.Customer.IUpdateCustomerHandler,
            RO.DevTest.Application.UseCases.Customer.Update.UpdateCustomerHandler>();

        // delete
        builder.Services.AddScoped<
            RO.DevTest.Application.Interfaces.UseCases.Customer.IDeleteCustomerHandler,
            RO.DevTest.Application.UseCases.Customer.Delete.DeleteCustomerHandler>();

        // register product use case handlers
        // create
        builder.Services.AddScoped<
            RO.DevTest.Application.Interfaces.UseCases.Product.ICreateProductHandler,
            RO.DevTest.Application.UseCases.Product.Create.CreateProductHandler>();

        // // update by code
        // builder.Services.AddScoped<
        //     RO.DevTest.Application.Interfaces.UseCases.Product.IUpdateProductByCodeHandler,
        //     RO.DevTest.Application.UseCases.Product.Update.UpdateProductByCodeHandler>();

        // // delete by code
        // builder.Services.AddScoped<
        //     RO.DevTest.Application.Interfaces.UseCases.Product.IDeleteProductByCodeHandler,
        //     RO.DevTest.Application.UseCases.Product.Delete.DeleteProductByCodeHandler>();

        // // get by id
        // builder.Services.AddScoped<
        //     RO.DevTest.Application.Interfaces.UseCases.Product.IGetProductByIdHandler,
        //     RO.DevTest.Application.UseCases.Product.Get.GetProductByIdHandler>();

        // // get by code
        // builder.Services.AddScoped<
        //     RO.DevTest.Application.Interfaces.UseCases.Product.IGetProductByCodeHandler,
        //     RO.DevTest.Application.UseCases.Product.Get.GetProductByCodeHandler>();

        // Add Mediatr to program
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ApplicationLayer).Assembly,
                typeof(Program).Assembly
            );
        });

        var app = builder.Build();

        // configure the http request pipeline
        if(app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
