using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApiServer.Demo
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public virtual List<Order> Orders { get; set; } = new List<Order>();
    }
}
