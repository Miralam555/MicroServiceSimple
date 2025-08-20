using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models.Context;
using Shared.Events;

namespace Order.API.Consumers
{
    public class StockNotReservedConsumer(OrrderAPIDbContext _context) : IConsumer<StockNotReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order is null)
                throw new NullReferenceException();
            order.OrderStatus = Enums.OrderStatus.Fail;
            await _context.SaveChangesAsync();
        }
    }
}
