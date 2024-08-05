using System;
using Newtonsoft.Json;
using System.Text;
using TimeTrackingClientMVC.Models;

namespace TimeTrackingClientMVC.Services
{
	public class RegisterTimeService : IRegisterTimeService
    {
		
            public async Task<HttpResponseMessage> PostTrack(TimeRegistrationDTO timeRegistrationDTO, HttpClient client)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(timeRegistrationDTO);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://localhost:7183/TimeRegistration/PostTimeregistration", content);
                    return response;
                }
                catch (Exception ex)
                {
                    // Log error or handle it as needed
                    throw new InvalidOperationException("Error serializing the track data.", ex);
                }
            }
        
	}

    
}

