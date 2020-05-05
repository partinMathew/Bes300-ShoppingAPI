using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApi.Mappers
{
    public class EfCurbsideMapper : IMapCurbisdeOrders
    {
        private readonly ShoppingDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;

        public EfCurbsideMapper(ShoppingDataContext context, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }

        public async Task<CurbsideOrder> GetOrderById(int id)
        {
            var order = await _context.CurbsideOrders.SingleOrDefaultAsync(order => order.Id == id);

            //TODO: use ProjectTo
            if(order == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<CurbsideOrder>(order);
            }
        }

        public async Task<CurbsideOrder> PlaceOrder(CreateCurbisdeOrder orderToPlace)
        {
            var order = _mapper.Map<OrderForCurbside>(orderToPlace);
            _context.CurbsideOrders.Add(order);
            await _context.SaveChangesAsync();
            var response = _mapper.Map<CurbsideOrder>(order);
            //Process each of the items

            return response;
        }
    }
}
