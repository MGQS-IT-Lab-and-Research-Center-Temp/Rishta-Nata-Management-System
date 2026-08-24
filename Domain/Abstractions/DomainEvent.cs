using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions;
public abstract class DomainEvent
{
    DateTime OccuredOn { get; set; } = DateTime.Now; 
}
