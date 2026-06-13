using Stock_Trading_Simulation_API.Domain_Model;

namespace Stock_Trading_Simulation_API.Application
{
    public interface IMatchingEngine
    {
        Task<List<Trade>> MatchAsync();
    }
}