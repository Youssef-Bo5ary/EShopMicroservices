using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
	public void Configure(EntityTypeBuilder<OrderItem> builder)
	{
		//to identify primary key
		builder.HasKey(oi => oi.Id);
		builder.Property(oi => oi.Id).HasConversion(
								   orderItemId => orderItemId.Value,
								   dbId => OrderItemId.Of(dbId));
		// 1 product : M orderItems
		builder.HasOne<Product>()
			.WithMany()
			.HasForeignKey(oi => oi.ProductId);
		builder.Property(oi => oi.Quantity).IsRequired();

		builder.Property(oi => oi.Price).IsRequired();
	}
}
