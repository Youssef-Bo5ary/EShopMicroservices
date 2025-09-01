using Carter;
using Catalog.API.Products.CreateProduct;
using Mapster;
using MediatR;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id ,string Name, List<string> Category, string Description, string ImageFile, decimal Price);

//Response
public record UpdateProductResponse(bool IsSuccess);
public class UpdateProductEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("/products", async (UpdateProductResult request, ISender sender) =>
		{
			
			var command = request.Adapt<UpdateProductCommand>();//by Mapster
			//send the command by mediator
			var result = await sender.Send(command);//by Mediator
			//response after product had updated
			var response = result.Adapt<UpdateProductResponse>(); //by Mapster
			//return the api to client to test it in postman
			return Results.Ok(response);
		})
			.WithName("UpdateProduct")
			.Produces<UpdateProductResponse>(StatusCodes.Status201Created)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.WithSummary("Update Product")
			.WithDescription("Update Product");
	}
}
