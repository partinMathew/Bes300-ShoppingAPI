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

        public ShoppingListController(ShoppingDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("shoppinglist")]
        public async Task<ActionResult> GetFullShoppingList()
        {
            var response = new GetShoppingListResponse();

            response.Data = await _dataContext.ShoppingItems
                .Select(item => new ShoppingListItemResponse
                {
                    Id = item.Id,
                    Description = item.Description,
                    Purchased = item.Purchased
                }).ToListAsync();

            return Ok(response);
        }
    }
}
