using System.Collections.Generic;

namespace Arbitrader.Core.Data
{
    public class Order
    {
        public int Id { get; set; }

        List<OrderItem> Transactions { get; set; }
    }
}
