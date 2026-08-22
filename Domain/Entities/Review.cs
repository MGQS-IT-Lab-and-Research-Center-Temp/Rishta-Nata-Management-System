using System;
using System.Collections.Generic;
using System.Text;
using Domain.Abstractions;
namespace Domain.Entities;

    public class Review : AuditableEntity
    {
        
        public string Title { get; set; }
        public string Comment { get; set; }
        public Guid MarriageApplicationId { get; set; } 
        public string Status { get; set; }
        public string ReviewedAt { get; set; }
        public FormApplication? MarriageApplication  { get; set; }
        public DateTime ReviewerId { get; set; }
    }

