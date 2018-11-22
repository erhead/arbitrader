using Arbitrader.Core.Data;
using System.Collections.Generic;

namespace Arbitrader.Core
{
    public interface IOrderManager
    {
        int PlaceOrder(IEnumerable<OrderItem> transactions, bool execute);

        Order GetOrder(int orderId);

        void ExecuteOrder(int orderId);

        bool CancelOrder(int orderId);
    }
}
