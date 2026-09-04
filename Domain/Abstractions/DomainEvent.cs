namespace Domain.Abstractions;

public abstract class DomainEvent
{
    DateTime OccuredOn { get; set; } = DateTime.Now; 
}
