using MassTransit;
using Shared;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<StockReservedEvent>
    {
        [EndpointName(RabbitMQSettings.Payment_StockReservedEventQueue)]
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            if (true)
            {
                
                PaymentCompletedEvent payment = new()
                {
                    OrderId = context.Message.OrderId
                };
                await publishEndpoint.Publish(payment);
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Kifayet qeder vesait yoxdur",
                    OrderItems = context.Message.OrderItems
                };
                await publishEndpoint.Publish(paymentFailedEvent);
                await Console.Out.WriteLineAsync("Odeme alinmadi..");
            }
        }
    }
}
