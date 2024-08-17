using DogRallyMVC.Models;
using Newtonsoft.Json;
using System.Text;

namespace DogRallyMVC.Services
{
    public class RegisterHours : IRegisterHours
    {
        public async Task<HttpResponseMessage> PostHours(TimetrackingDTO tevm, HttpClient client)
        {
            try
            {
                var json = JsonConvert.SerializeObject(tevm);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://xn--tkketidapi-b8d5gxdgbee6btgp-wlc.northeurope-01.azurewebsites.net/TimeTracking/PostHours", content);
                return response;
            }
            catch (Exception ex)
            {
                // Log error or handle it as needed
                throw new InvalidOperationException("Error serializing the track data.", ex);
            }
        }

        // Ny metode til at hente timer fra API'en
        public async Task<List<TimetrackingDTO>> GetHours(HttpClient client)
        {
            try
            {
                var response = await client.GetAsync("https://xn--tkketidapi-b8d5gxdgbee6btgp-wlc.northeurope-01.azurewebsites.net/TimeTracking/GetHours");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var timetrackingList = JsonConvert.DeserializeObject<List<TimetrackingDTO>>(jsonResponse);
                return timetrackingList;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error fetching the timetracking data.", ex);
            }
        }

        // Ny metode til kun at hente brugeren timer fra API'en
        public async Task<List<TimetrackingDTO>> GetUserHours(HttpClient client)
        {
            try
            {
                var response = await client.GetAsync("https://xn--tkketidapi-b8d5gxdgbee6btgp-wlc.northeurope-01.azurewebsites.net/TimeTracking/GetUserHours");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var timetrackingList = JsonConvert.DeserializeObject<List<TimetrackingDTO>>(jsonResponse);
                return timetrackingList;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error fetching the timetracking data.", ex);
            }
        }
    }
}
