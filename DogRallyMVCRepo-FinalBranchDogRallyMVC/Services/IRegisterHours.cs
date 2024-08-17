using DogRallyMVC.Models;

namespace DogRallyMVC.Services
{
    public interface IRegisterHours
    {
        Task<HttpResponseMessage> PostHours(TimetrackingDTO tevm, HttpClient client);
        Task<List<TimetrackingDTO>> GetHours(HttpClient client);
        Task<List<TimetrackingDTO>> GetUserHours(HttpClient client);

    }
}