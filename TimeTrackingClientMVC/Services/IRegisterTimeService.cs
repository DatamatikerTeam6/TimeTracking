using TimeTrackingClientMVC.Models;

namespace TimeTrackingClientMVC.Services
{
    public interface IRegisterTimeService
    {
        Task<HttpResponseMessage> PostTrack(TimeRegistrationDTO timeRegistrationDTO, HttpClient client);
    }
}
