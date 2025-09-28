using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Abstractions;

public interface IDomainEvent : INotification
{
	// this interface responsible for sending the events to the handler
	Guid EventId => Guid.NewGuid();
	public DateTime OccurredOn => DateTime.Now;
	public string EventType => GetType().AssemblyQualifiedName;//get the actual name
	//of the class so it become a unique type name
}
