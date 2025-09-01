using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Catalog.API.Data;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//dependency injection
builder.Services.AddMediatR(config =>
{
	config.RegisterServicesFromAssembly(typeof(Program).Assembly);
	config.AddOpenBehavior(typeof(ValidationBehavior<,>));// add a pipeline behaviour
	config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});//dependency injection for fluent validation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);


builder.Services.AddCarter();
builder.Services.AddMarten(Opts =>
{
	Opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
if (builder.Environment.IsDevelopment())
	builder.Services.InitializeMartenWith<CatalogInitialData>();
//use LightweightSession to secure crud operations

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
// dependency injection for Health checks
builder.Services.AddHealthChecks()
	.AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler(options => { });
//use Health Checks
app.UseHealthChecks("/health",
	new HealthCheckOptions
	{ 
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});



app.Run();


