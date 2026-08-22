using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces.Service
{

    public interface IReferenceNumberService
    {
        Task<string> GenerateAsync();
    }
}
