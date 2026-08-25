using System;
using System.Collections.Generic;
using System.Text;

using Domain.Abstractions;
using global::Domain.Abstractions;

namespace Domain.Entities;

    public class Notification : AuditableEntity
    {
        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string ActionUrl { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

    }

