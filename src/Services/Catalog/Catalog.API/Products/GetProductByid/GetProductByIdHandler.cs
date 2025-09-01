using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Catalog.API.Products.GetProducts;
using Marten;
using Marten.Linq.QueryHandlers;

namespace Catalog.API.Products.GetProductByid;

public record GetProductIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler
	(IDocumentSession session )
	: IQueryHandler<GetProductIdQuery, GetProductByIdResult>
{
	public async Task<GetProductByIdResult> Handle(GetProductIdQuery query, CancellationToken cancellationToken)
	{
		var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

		if (product is null)
		{
			throw new ProductNotFoundException(query.Id);
		}
		return new GetProductByIdResult(product);
	}
}
