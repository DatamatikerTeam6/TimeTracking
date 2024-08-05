using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTrackingClientMVC.Models;
using TimeTrackingClientMVC.Services;

namespace TimeTrackingClientMVC.Controllers
{
    public class TimeTracking : Controller
    {
        private readonly IRegisterTimeService _registerTimeService;
        private readonly HttpClient _httpClient;

        public TimeTracking(IRegisterTimeService registerTimeService, HttpClient httpClient)
        {
            _registerTimeService = registerTimeService;
            _httpClient = httpClient;
        }

        // GET: RegisterHours
        public ActionResult RegisterHours()
        {
            return View(new TimeRegistrationDTO());
        }


        [HttpPost]
        public async Task<ActionResult> RegisterHours(TimeRegistrationDTO timeRegistration)
        {
            if (ModelState.IsValid)
            {
                var response = await _registerTimeService.PostTrack(timeRegistration, _httpClient);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Data registreret succesfuldt!";
                }
                else
                {
                    ViewBag.Message = "Fejl ved registrering af data.";
                }
            }
            return View(timeRegistration);
        }
    }
}




    

