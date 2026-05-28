namespace Stock_Trading_Simulation_API.Domain_Model
{
    public enum OrderSide
    {
        Buy,
        Sell
    }

    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Symbol { get; set; } = string.Empty;
        public OrderSide Side { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
