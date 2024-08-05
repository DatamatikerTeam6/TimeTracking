using DogRallyMVC.Models;

namespace DogRallyMVC.Services
{
    public interface IRegisterHours
    {
        Task<HttpResponseMessage> PostHours(TimetrackingDTO tevm, HttpClient client);
    }
}