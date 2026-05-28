namespace Stock_Trading_Simulation_API.Domain_Model
{
    public class Trade
    {
        public Guid BuyOrderId { get; set; }
        public Guid SellOrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    }
}
