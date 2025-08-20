using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Stock.API.Models;
using Stock.API.Services;

namespace Stock.API.Consumers
{

    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        readonly MongoDBService mongoDBService;
        readonly ISendEndpointProvider sendEndpointProvider;
        readonly IPublishEndpoint publishEndpoint;
        public OrderCreatedEventConsumer(MongoDBService mongoDBService, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            this.mongoDBService = mongoDBService;
            this.sendEndpointProvider = sendEndpointProvider;
            this.publishEndpoint = publishEndpoint;
        }
        [EndpointName("order-created-queue")]
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            IMongoCollection<Models.Stock> collection=mongoDBService.GetCollection<Models.Stock>();
            foreach (var item in context.Message.OrderItemMessages)
            {
                stockResult.Add(await (await collection.FindAsync(s => s.ProductId == item.ProductId.ToString() && s.Count >(long)item.Count)).AnyAsync());
            }
            if (stockResult.TrueForAll(s => s.Equals(true)))
            {
                //StockUpdates
                foreach (var item in context.Message.OrderItemMessages)
                {
                   Models.Stock stock= await (await collection.FindAsync(s => s.ProductId == item.ProductId.ToString())).FirstOrDefaultAsync();
                    var fd = item;
                    stock.Count -= item.Count;
                    await collection.FindOneAndReplaceAsync(x => x.ProductId == item.ProductId.ToString(), stock);
                }
                var sendEndpoint= await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
                StockReservedEvent stockReservedEvent = new()
                {
                    BuyerId=context.Message.BuyerId,
                    OrderId=context.Message.OrderId,
                    TotalPrice=context.Message.TotalPrice,
                    OrderItems=context.Message.OrderItemMessages
                };
                await sendEndpoint.Send(stockReservedEvent);
            }
            else
            {
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "Stokda mehsul yoxdur"
                };
                await publishEndpoint.Publish(stockNotReservedEvent);
            }
        }
    }
}
