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

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

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

        // update 
        builder.Services.AddScoped<
            RO.DevTest.Application.Interfaces.UseCases.Product.IUpdateProductHandler,
            RO.DevTest.Application.UseCases.Product.Update.UpdateProductHandler>();

        // delete by id
        builder.Services.AddScoped<
            RO.DevTest.Application.Interfaces.UseCases.Product.IDeleteProductHandler,
            RO.DevTest.Application.UseCases.Product.Delete.DeleteProductHandler>();

        // // get by id
        // builder.Services.AddScoped<
        //     RO.DevTest.Application.Interfaces.UseCases.Product.IGetProductByIdHandler,
        //     RO.DevTest.Application.UseCases.Product.Get.GetProductByIdHandler>();

        // // get by code
        // builder.Services.AddScoped<
        //     RO.DevTest.Application.Interfaces.UseCases.Product.IGetProductByCodeHandler,
        //     RO.DevTest.Application.UseCases.Product.Get.GetProductByCodeHandler>();

        // register sale
        builder.Services.AddScoped<
            RO.DevTest.Application.Interfaces.UseCases.Sale.ICreateSaleHandler,
            RO.DevTest.Application.UseCases.Sale.Create.CreateSaleHandler>();

        // update sale
        builder.Services.AddScoped<
            RO.DevTest.Application.Interfaces.UseCases.Sale.IUpdateSaleHandler,
            RO.DevTest.Application.UseCases.Sale.Update.UpdateSaleHandler>();

        // delete sale
        builder.Services.AddScoped<
            RO.DevTest.Application.Interfaces.UseCases.Sale.IDeleteSaleHandler,
            RO.DevTest.Application.UseCases.Sale.Delete.DeleteSaleHandler>();


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
