using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Messaging.MassTransit;
using Carter;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container

//Application Services
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{ 
	config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
	config.AddOpenBehavior(typeof(ValidationBehavior<,>));
	config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});

//Data Service
builder.Services.AddMarten(opts =>
{
	opts.Connection(builder.Configuration.GetConnectionString("Database")!);
	opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
//Scrutor Package
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = builder.Configuration.GetConnectionString("Redis");
	//options.InstanceName = "Basket";
});

//Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
	options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
	// related to ssl certificate for docker compose
	.ConfigurePrimaryHttpMessageHandler(() =>
	{
		var handler = new HttpClientHandler
		{
			ServerCertificateCustomValidationCallback =
			HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
		};

		return handler;
	});

//Async Communication Services
builder.Services.AddMessageBroker(builder.Configuration);

//Cross Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
	 .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
	.AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();


//Configure the Http request pipeline
app.MapCarter();

app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
	new HealthCheckOptions
	{
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});
app.Run();
