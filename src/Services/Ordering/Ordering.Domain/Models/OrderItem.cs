namespace Ordering.Domain.Models;

public class OrderItem : Entity<OrderItemId>
{
	internal OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
	{//we gonna use this constructor in order class as order is Aggregate so there is a method in order
	 //, responsible for creating an order item

		Id = OrderItemId.Of(Guid.NewGuid());
		OrderId = orderId;
		ProductId = productId;
		Quantity = quantity;
		Price = price;
	}

	public OrderId OrderId { get; private set; } = default!;
	public ProductId ProductId { get; private set; } = default!;
	public int Quantity { get; private set; } = default!;
	public decimal Price { get; private set; } = default!;
}
