﻿using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Catalog.API.Products.GetProducts;
using Catalog.API.Products.UpdateProduct;
using FluentValidation;
using Marten;
using System.Windows.Input;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
	public DeleteProductCommandValidator()
	{
		RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required");
	}
}
internal class DeleteProductCommandHandler
	(IDocumentSession session)
	: ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
	public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
	{

		session.Delete<Product>(command.Id);
		await session.SaveChangesAsync(cancellationToken);
		return new DeleteProductResult(true);
	}
}
