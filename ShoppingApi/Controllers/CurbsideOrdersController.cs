using Microsoft.AspNetCore.Mvc;
using ShoppingApi.Data;
using ShoppingApi.Mappers;
using ShoppingApi.Models;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class CurbsideOrdersController : ControllerBase
    {
        private readonly IMapCurbisdeOrders _curbsideMapper;
        private readonly CurbsideChannel _channel;
        private readonly ShoppingDataContext _context;

        public CurbsideOrdersController(IMapCurbisdeOrders curbsideMapper, CurbsideChannel channel, ShoppingDataContext context)
        {
            _curbsideMapper = curbsideMapper;
            _channel = channel;
            _context = context;
        }

        [HttpPost("curbsideordersync")]
        public async Task<ActionResult> PlaceOrderSynchronously([FromBody] CreateCurbisdeOrder orderToPlace)
        {
            var temp = await _curbsideMapper.PlaceOrder(orderToPlace);
            for(var t = 0; t < temp.Items.Count; t++)
            {
                Thread.Sleep(1000);
            }
            temp.Status = Data.CurbsideOrderStatus.Processed;
            var order = await _context.SaveChangesAsync();
            return Ok(temp); // not going to map it... just want you to see.
        }


        [HttpPost("curbsideorders")]
        public async Task<ActionResult> PlaceOrder([FromBody] CreateCurbisdeOrder orderToPlace)
        {
            //validate the model (return 400 if bad)
            CurbsideOrder response = await _curbsideMapper.PlaceOrder(orderToPlace);
           
            // this should be wrapped in a try catch and if fails remove it from the database etc.
            await _channel.AddCurbsideOrder(new CurbsideChannelRequest { OrderId = response.Id });
            return Ok(response);
        }

        [HttpGet("curbsideorders/{id:int}")]
        public async Task<ActionResult<CurbsideOrder>> GetById(int id)
        {
            CurbsideOrder response = await _curbsideMapper.GetOrderById(id);
            if(response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
