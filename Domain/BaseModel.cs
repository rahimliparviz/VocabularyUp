using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class BaseModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}