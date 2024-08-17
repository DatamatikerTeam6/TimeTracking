using DogRallyMVC.Models;
namespace DogRallyMVC.Services
{
    public interface IGetUserTracksFromAPI
    {
        Task<List<TimetrackingDTO>> GetUserTracks(HttpClient client, string userID);

        Task<List<TimetrackingDTO>> GetAllUserTracks(HttpClient client);
    }
}