using System.ComponentModel.DataAnnotations;

namespace RestApiServer.Demo
{

    public class Order
    {
        [Key]
        public string Id { get; set; }

        public decimal Amount { get; set; }

        public long CustomerId { get; set; }
    }
}
