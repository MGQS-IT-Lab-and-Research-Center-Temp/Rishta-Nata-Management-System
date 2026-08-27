// File: Domain/Abstractions/IEventHandler.cs
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
	public interface IEventHandler<in TEvent> where TEvent : DomainEvent
	{
		Task Handle(TEvent domainEvent, CancellationToken cancellationToken = default);
	}
}
