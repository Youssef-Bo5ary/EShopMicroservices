using BuildingBlocks.CQRS;
using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Products.CreateProduct
{
	//Request
	public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

	//Response
	public record CreateProductResponse(Guid Id);

	public class CreateProductEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
			{
				//create the command to create a product
				var command = request.Adapt<CreateProductCommand>();
				//send the command by mediator
				var result = await sender.Send(command);
				//response after product had created
				var response = result.Adapt<CreateProductResponse>();
				//return the api to client to test it in postman
				return Results.Created($"/products/{response.Id}", response);
			})
				.WithName("CreateProduct")
				.Produces<CreateProductResponse>(StatusCodes.Status201Created)
				.ProducesProblem(StatusCodes.Status400BadRequest)
				.WithSummary("Create Product")
				.WithDescription("Create Product");
		}
	}
}
