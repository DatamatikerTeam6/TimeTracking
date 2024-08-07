using DogRallyMVC.Models;
using DogRallyMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace DogRallyMVC.Controllers
{
    public class TimetrackingController : Controller
    {
        // Dependency Injection
        private readonly ILogger<TimetrackingController> _logger;
        private readonly IRegisterHours _registerHours;
        private readonly IGetUserTracksFromAPI _getUserTracksFromAPI;
        private readonly IDeleteTrackFromAPI _deleteTrackFromAPI;
        private readonly IHttpClientFactory _httpClientFactory;

        public TimetrackingController(ILogger<TimetrackingController> logger, IRegisterHours postTrackToAPI, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _registerHours = postTrackToAPI;
            _httpClientFactory = httpClientFactory;
            
        }


        private void ConfigureHttpClientWithToken(HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }


        


        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RegisterHours()
        {
            var client = _httpClientFactory.CreateClient();

            // Get the current user's userID
            var userID = HttpContext.Session.GetString("UserID");

            // Create a TimetrackingDTO with the userID
            var viewModel = new TimetrackingDTO
            {
                UserID = userID,
                Date = DateTime.Now,
                HoursWorked = 0 // Default value or initialize as needed
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> RegisterHours([Bind("Date, HoursWorked, UserID")] TimetrackingDTO dto)
        {
            var client = _httpClientFactory.CreateClient();

            // Get JWT token from session
            string token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "JWT token is missing.");
                ViewBag.ApiResponse = "JWT token is missing.";
                return View(dto);
            }

            // Configure HttpClient with JWT token for this request
            ConfigureHttpClientWithToken(client, token);

            try
            {
                // Assuming _postTrackToAPI.PostTrack now accepts TimetrackingDTO
                var response = await _registerHours.PostHours(dto, client);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.SuccessMessage = "Dine timer blev registréret korrekt";
                   
                }
                else
                {
                    // Read the response body for error details
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"API returned an error: {errorResponse}");
                    ViewBag.ApiResponse = $"API Error: {errorResponse}";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error sending data to API: {ex.Message}");
                ViewBag.ApiResponse = $"Exception: {ex.Message}";
            }

            return View(dto); // Return to the view with errors and ViewBag information
        }

        //Get all user hours
        [HttpGet]
        public async Task<IActionResult> ViewAllUserHours()
        {
            var client = _httpClientFactory.CreateClient();

            // Get JWT token from session
            string token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "JWT token is missing.");
                return View(new List<TimetrackingDTO>());
            }

            // Configure HttpClient with JWT token for this request
            ConfigureHttpClientWithToken(client, token);

            try
            {
                var hoursList = await _registerHours.GetHours(client);
                // Sort the list by date in descending order
                var sortedHoursList = hoursList.OrderByDescending(h => h.Date).ToList();

                return View(sortedHoursList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching data from API: {ex.Message}");
                return View(new List<TimetrackingDTO>());
            }
        }

        //Get user hours
        [HttpGet]
        public async Task<IActionResult> ViewUserHours()
        {
            var client = _httpClientFactory.CreateClient();

            // Get JWT token from session
            string token = HttpContext.Session.GetString("JWTToken");

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError(string.Empty, "JWT token is missing.");
                return View(new List<TimetrackingDTO>());
            }

            // Configure HttpClient with JWT token for this request
            ConfigureHttpClientWithToken(client, token);

            try
            {
                var hoursList = await _registerHours.GetUserHours(client);
                // Sort the list by date in descending order
                var sortedHoursList = hoursList.OrderByDescending(h => h.Date).ToList();

                return View(sortedHoursList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching data from API: {ex.Message}");
                return View(new List<TimetrackingDTO>());
            }
        }


        //[HttpGet]
        //public async Task<IActionResult> ReadTrack(int id)
        //{
        //    var client = _httpClientFactory.CreateClient();

        //    string token = HttpContext.Session.GetString("JWTToken");

        //    // Configure HttpClient with JWT token for this request
        //    ConfigureHttpClientWithToken(client, token);

        //    //Get Track from API
        //    List<TrackExerciseDTO> trackExercises = await _getTrackFromAPI.GetTrack(client, id);

        //    foreach (var trackExerciseDTO in trackExercises)
        //    {
        //        Console.WriteLine((trackExerciseDTO.ExerciseIllustrationPath).ToString());
        //    }

        //    foreach (var trackExerciseDTO in trackExercises)
        //    {
        //        _logger?.LogInformation((trackExerciseDTO.ExerciseIllustrationPath).ToString());
        //    }

        //    return View(trackExercises);
        //}

        //[HttpGet]
        //public async Task<IActionResult> UpdateTrack(int id)
        //{
        //    var client = _httpClientFactory.CreateClient();

        //    string token = HttpContext.Session.GetString("JWTToken");
        //    // Configure HttpClient with JWT token for this request
        //    ConfigureHttpClientWithToken(client, token);


        //    // Get Track from API
        //    List<TrackExerciseDTO> trackExercises = await _getTrackFromAPI.GetTrack(client, id);


        //    //Get the current user's userID
        //    var userID = HttpContext.Session.GetString("UserID");

        //    // Assuming the API returns all exercises for a specific track
        //    // and assuming TrackDTO and ExerciseDTO can be constructed from TrackExerciseDTO
        //    var trackDTO = new TimetrackingDTO
        //    {
        //        UserID = userID,
        //        TrackID = trackExercises.FirstOrDefault()?.ForeignTrackID ?? 0,
        //        TrackName = trackExercises.FirstOrDefault()?.TrackName
        //    };

        //    var exerciseDTOs = trackExercises.Select(te => new ExerciseDTO
        //    {
        //        ExerciseID = te.ForeignExerciseID,
        //        ExerciseIllustrationPath = te.ExerciseIllustrationPath,
        //        ExercisePositionX = te.TrackExercisePositionX,
        //        ExercisePositionY = te.TrackExercisePositionY
        //    }).ToList();

        //    TrackExerciseViewModelDTO viewModel = new TrackExerciseViewModelDTO
        //    {
        //        Track = trackDTO,
        //        Exercises = exerciseDTOs
        //    };

        //    return View(viewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateTrack([Bind("Exercises, Track")] TrackExerciseViewModelDTO tevm)
        //{
        //    HttpClient client = _httpClientFactory.CreateClient();
        //    string token = HttpContext.Session.GetString("JWTToken");
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return Unauthorized();
        //    }

        //    // Configure HttpClient with JWT token for this request
        //    ConfigureHttpClientWithToken(client, token);

        //    // Get the user's role from the token
        //    var userRole = GetUserRoleFromToken(token);
        //    var userId = HttpContext.Session.GetString("UserID");

        //    string json = JsonConvert.SerializeObject(tevm);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    _logger.LogInformation(json);

        //    try
        //    {
        //        var response = await client.PutAsync("https://localhost:7183/Tracks/UpdateTrack", content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            if (userRole == "Admin")
        //            {
        //                return RedirectToAction("Tracks", new { type = "AllTracks" });
        //            }
        //            else
        //            {
        //                return RedirectToAction("Tracks", new { type = "MyTracks" });
        //            }                 
        //        }
        //        else
        //        {
        //            // Read the response body for error details
        //            var errorResponse = await response.Content.ReadAsStringAsync();
        //            ModelState.AddModelError(string.Empty, $"API returned an error: {errorResponse}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Error sending data to API: {ex.Message}");
        //    }
        //    return View(tevm); // Return to the view with errors and ViewBag information
        //}



        //[HttpPost]
        //public async Task<IActionResult> DeleteTrack(int id)
        //{
        //    var client = _httpClientFactory.CreateClient();
        //    string token = HttpContext.Session.GetString("JWTToken");
        //    // Configure HttpClient with JWT token for this request
        //    ConfigureHttpClientWithToken(client, token);

        //    var userRole = GetUserRoleFromToken(token);
        //    try
        //    {              
        //        await _deleteTrackFromAPI.DeleteTrack(client, id);
        //        if (userRole == "Admin")
        //        {
        //            return RedirectToAction("Tracks", new { type = "AllTracks" });
        //        }
        //        else
        //        { 
        //            return RedirectToAction("Tracks", new { type = "MyTracks" }); 
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Error", new { message = ex.Message });
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetUserRoleFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
            var role = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
            return role;
        }
    }
}











