// File: Domain/Abstractions/IEventHandler.cs
namespace Domain.Abstractions;

public interface IEventHandler<in TEvent> where TEvent : DomainEvent
{
	Task Handle(TEvent domainEvent, CancellationToken cancellationToken = default);
}
