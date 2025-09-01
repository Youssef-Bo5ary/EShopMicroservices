using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Catalog.API.Products.GetProducts;
using FluentValidation;
using Marten;
using MediatR;
using System.Windows.Input;

namespace Catalog.API.Products.CreateProduct;

//command Query 
public record CreateProductCommand(string Name , List<string> Category, string Description, string ImageFile ,decimal Price)
  :ICommand<CreateProductResult>;
// result object
public record CreateProductResult(Guid Id);

//Validation Class
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
	public CreateProductCommandValidator()
	{
		RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
		RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
		RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Image is required");
		RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
	}
}
internal class CreateProductCommandHandler(IDocumentSession session)
	: ICommandHandler<CreateProductCommand, CreateProductResult>
{
	public async Task<CreateProductResult> Handle(CreateProductCommand command,
		CancellationToken cancellationToken)
	{
		//create Product entity from command object (includes the name category ...etc of the product)


		var product = new Product
		{
			Name = command.Name,
			Category = command.Category,
			Description = command.Description,
			ImageFile = command.ImageFile,
			Price = command.Price
		};

		//save the database
		session.Store(product);
		await session.SaveChangesAsync(cancellationToken);
		//return createProductResult result
		return new CreateProductResult(product.Id);
	}
}
