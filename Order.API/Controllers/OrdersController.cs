using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.Models.Entities;
using Order.API.ViewModels;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    readonly OrderAPIDbContext _dbContext;

    public OrdersController(OrderAPIDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderVM input)
    {
        Models.Entities.Order order = new()
        {
            OrderId = Guid.NewGuid(),
            BuyerId = input.BuyerId,
            CreatedDate = DateTime.Now,
            OrderStatu = Models.Enums.OrderStatus.Suspended
        };

        order.OrderItems = input.OrderItems.Select(x => new OrderItem
        {
            Count = x.Count,
            Price = x.Price,
            ProductId = x.ProductId,

        }).ToList();

        order.TotalPrice = input.OrderItems.Sum(x => x.Price * x.Count);


        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        return Ok();

    }

}
