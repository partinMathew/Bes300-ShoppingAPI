using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApi.Controllers
{
    public class ShoppingListController : ControllerBase
    {
        private readonly ShoppingDataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;

        public ShoppingListController(ShoppingDataContext dataContext, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }

        [HttpGet("shoppinglist")]
        public async Task<ActionResult> GetFullShoppingList()
        {
            var response = new GetShoppingListResponse();

            response.Data = await _dataContext.ShoppingItems
                .ProjectTo<ShoppingListItemResponse>(_mapperConfig).ToListAsync();

               

            return Ok(response);
        }
    }
}
