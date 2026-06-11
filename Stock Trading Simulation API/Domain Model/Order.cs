namespace Stock_Trading_Simulation_API.Domain_Model
{
    public enum OrderSide
    {
        Buy,
        Sell
    }

    public enum OrderStatus
    {
        Pending,
        Queued,
        PartiallyFilled,
        Filled,
        Cancelled,
        Rejected
    }

    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Symbol { get; set; } = string.Empty;
        public OrderSide Side { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        //Order status
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        // Original quantity submitted
        public int OriginalQuantity { get; set; }
    }
}
