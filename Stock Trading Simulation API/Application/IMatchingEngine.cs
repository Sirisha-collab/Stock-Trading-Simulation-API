using Stock_Trading_Simulation_API.Domain_Model;
using Stock_Trading_Simulation_API.Infrastrcuture;

namespace Stock_Trading_Simulation_API.Application
{
    public interface IMatchingEngine
    {
        Task Run(CancellationToken token);
        Task GenerateOrders(OrderQueue queue);
        Task<List<Trade>> MatchAsync();    
    }
}
