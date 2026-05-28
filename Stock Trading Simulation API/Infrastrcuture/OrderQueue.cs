using Stock_Trading_Simulation_API.Domain_Model;
using System.Threading.Channels;

namespace Stock_Trading_Simulation_API.Infrastrcuture
{
    public class OrderQueue
    {
        private readonly Channel<Order> _channel;
        public OrderQueue()
        {
            _channel = Channel.CreateUnbounded<Order>(
                new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false
                });
        }

        public ValueTask Enqueue(Order order)
            => _channel.Writer.WriteAsync(order);

        public IAsyncEnumerable<Order> ReadAll(CancellationToken ct)
            => _channel.Reader.ReadAllAsync(ct);
    }
}
