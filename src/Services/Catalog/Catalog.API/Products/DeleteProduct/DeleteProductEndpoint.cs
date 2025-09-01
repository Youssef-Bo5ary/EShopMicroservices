using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Products.DeleteProduct;

public record UpdateProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("/products/{id}", async (Guid id , ISender sender) =>
		{
			var result = await sender.Send(new DeleteProductCommand(id));

			var response = result.Adapt<UpdateProductResponse>();
			return Results.Ok(response);
		})
			.WithName("DeleteProduct")
			.Produces<UpdateProductResponse>(StatusCodes.Status201Created)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.WithSummary("Delete Product")
			.WithDescription("Delete Product");
	}
}
