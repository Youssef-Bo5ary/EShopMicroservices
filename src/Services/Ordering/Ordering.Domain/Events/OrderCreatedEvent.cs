using Ordering.Domain.Models;

namespace Ordering.Domain.Events;

//this event after making order will sent to the handler
public record OrderCreatedEvent(Order order) : IDomainEvent;
