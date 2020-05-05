using Microsoft.AspNetCore.Mvc;
using ShoppingApi.Mappers;
using ShoppingApi.Models;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class CurbsideOrdersController : ControllerBase
    {
        private readonly IMapCurbisdeOrders _curbsideMapper;
        private readonly CurbsideChannel _channel;

        public CurbsideOrdersController(IMapCurbisdeOrders curbsideMapper, CurbsideChannel channel)
        {
            _curbsideMapper = curbsideMapper;
            _channel = channel;
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
