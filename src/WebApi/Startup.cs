using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application.Order.CancelOrder;
using FavoDeMel.Application.ManageService.Application.Order.PlaceNewOrder;
using FavoDeMel.Application.ManageService.Application.Order.ReprioritizeOrder;
using FavoDeMel.Application.ManageService.Application.Order.ToDoneOrder;
using FavoDeMel.Application.ManageService.Application.Order.ToOrderInDelivery;
using FavoDeMel.Application.ManageService.Application.Order.ToOrderInPreparation;
using FavoDeMel.Application.ManageService.Application.OrderTab.CloseOrderTab;
using FavoDeMel.Application.ManageService.Application.OrderTab.OpenOrderTab;
using FavoDeMel.Application.ManageService.Core;
using FavoDeMel.Application.ManageService.Infrastructure.Repositories;
using FavoDeMel.Application.ManageService.Queries.Order.GetAllOrders;
using FavoDeMel.Application.ManageService.Queries.Order.GetById;
using FavoDeMel.Application.ManageService.Queries.Order.GetByPosition;
using FavoDeMel.Application.ManageService.Queries.Order.GetOrderByOrderTabId;
using FavoDeMel.Application.ManageService.Queries.OrderTab.GetAllOrderTab;
using FavoDeMel.Application.ManageService.Queries.OrderTab.GetOrderTabById;
using FavoDeMel.Infrastructure.Common.Exception;
using FavoDeMel.Infrastructure.Common.IdGenerator;
using FavoDeMel.Infrastructure.Data;
using FavoDeMel.WebApi.Workers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NATS.Client;
using STAN.Client;

namespace FavoDeMel.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            var connectionString = Configuration["ConnectionString"] ?? throw new NullReferenceException("The connection string is null.");
            services.AddSingleton(service => new ConnectionString(connectionString));
            services.AddSingleton<IIdGenerator, IdGenerator>(service => new IdGenerator(10));

            services.AddSingleton<IOrderRepositoryInMemory, OrderRepositoryInMemory>(provider => new OrderRepositoryInMemory());
            services.AddSingleton<IOrderTabRepositoryInMemory, OrderTabRepositoryInMemory>(provider =>
                new OrderTabRepositoryInMemory());

            services.AddSingleton(new ConnectionFactory());
            services.AddScoped(provider => provider.GetService<ConnectionFactory>().CreateConnection("nats://localhost:4222"));
            
            services.AddSingleton(new StanConnectionFactory());
            //TODO:...
            services.AddSingleton(provider =>provider.GetService<StanConnectionFactory>().CreateConnection("test-cluster", "application", StanOptions.GetDefaultOptions()));
                

            services.AddScoped(provider => new GetAllOrdersQuery(provider.GetService<IOrderRepositoryInMemory>()));
            services.AddScoped(provider => new GetByIdQuery(provider.GetService<IOrderRepositoryInMemory>()));
            services.AddScoped(provider => new GetByPositionQuery(provider.GetService<IOrderRepositoryInMemory>()));
            services.AddScoped(provider => new GetOrderByOrderTabIdQuery(provider.GetService<IOrderRepositoryInMemory>()));
            
            services.AddScoped(provider => new GetAllOrderTabQuery(provider.GetService<IOrderTabRepositoryInMemory>()));
            services.AddScoped(provider => new GetOrderTabByIdQuery(provider.GetService<IOrderTabRepositoryInMemory>()));

            services.AddScoped<OpenOrderTabUseCase>();
            services.AddScoped<CloseOrderTabUseCase>();

            services.AddSingleton<OrdersManager>();
            services.AddScoped<CancelOrderHandler>();
            services.AddScoped<PlaceNewOrderHandler>();
            services.AddScoped<ReprioritizeOrderHandler>();
            services.AddScoped<ToDoneOrderHandler>();
            services.AddScoped<ToOrderInDeliveryHandler>();
            services.AddScoped<ToOrderInPreparationHandler>();
            services.AddHostedService<OrderRequestProcessorHostedService>();

            services.AddHostedService<PersistOrderHostedService>();
            services.AddHostedService<PersistOrderTabHostedService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Favo de mel API"
                });
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                
                c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));

                c.TagActionsBy(api => api.GroupName);
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Favo de mel API V1");
            });
            
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();

                    if (!(exceptionHandler?.Error is CustomException exception))
                    {
                        context.Response.StatusCode = 500;
                        return;
                    }

                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";
                    var payload = JsonSerializer.Serialize(exception.Message);
                    await context.Response.WriteAsync(payload);
                });
            });
            
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
