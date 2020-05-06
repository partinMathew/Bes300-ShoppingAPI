using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ShoppingApi.Mappers;
using ShoppingApi.Models;
using ShoppingApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Hubs
{
    public class CurbsideHub : Hub
    {
        private readonly IMapCurbisdeOrders _curbsideMapper;
        private readonly ILogger<CurbsideHub> _logger;
        private readonly CurbsideChannel _channel;
        private readonly IMapper _mapper;

        public CurbsideHub(IMapCurbisdeOrders curbsideMapper, ILogger<CurbsideHub> logger, CurbsideChannel channel, IMapper mapper)
        {
            _curbsideMapper = curbsideMapper;
            _logger = logger;
            _channel = channel;
            _mapper = mapper;
        }

        public async Task PlaceOrder(CreateCurbisdeOrder orderToBePlaced)
        {
            var response = await _curbsideMapper.PlaceOrder(orderToBePlaced);
            await _channel.AddCurbsideOrder(new CurbsideChannelRequest { OrderId = response.Id, ClientId = Context.ConnectionId });
            await Clients.Caller.SendAsync("OrderPlaced", _mapper.Map<CurbsideOrder>(response));
        }
    }
}
