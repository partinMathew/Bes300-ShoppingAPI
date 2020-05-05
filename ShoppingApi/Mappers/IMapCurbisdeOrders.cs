using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Mappers
{
    public interface IMapCurbisdeOrders
    {
        Task<CurbsideOrder> PlaceOrder(CreateCurbisdeOrder orderToPlace);
        Task<CurbsideOrder> GetOrderById(int id);
    }
}
