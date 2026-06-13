namespace Stock_Trading_Simulation_API.Domain_Model
{
    public class DashboardDTO
    {
        public string Symbol { get; set; } = string.Empty;

        public decimal? LastPrice { get; set; }
        public DateTime? LastTradeTime { get; set; }

        public decimal? BestBid { get; set; }
        public decimal? BestAsk { get; set; }

        public int ActiveOrderCount { get; set; }

        public int? Spread =>
            BestBid.HasValue && BestAsk.HasValue
                ? (int)(BestAsk.Value - BestBid.Value)
                : null;
    }
}