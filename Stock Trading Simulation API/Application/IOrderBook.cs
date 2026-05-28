using Stock_Trading_Simulation_API.Domain_Model;

//define a contract for Order that an implementing class must provide, without dictating the implementation details. 
namespace Stock_Trading_Simulation_API.Application
{
    public interface IOrderBook
    {
        Task AddOrderAsync(Order order);
        bool HasMatch();
        Order GetBestBuy();
        Order GetBestSell();
        void RemoveBestBuy();
        void RemoveBestSell();
        List<Order> GetAllOrders();

    }
}
