using DogRallyAPI.Data;
using DogRallyAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace DogRallyAPI.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TimeTrackingController : ControllerBase
    {
        private readonly TimeTrackingDbContext _context;

        public TimeTrackingController(TimeTrackingDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles="Admin, User")]
        [HttpPost("PostHours")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateTrack([FromBody] TimetrackingDTO timetrackingDTO)
        {
            if (ModelState.IsValid)
            {
                var timetracking = new Timetracking
                {
                    Date = timetrackingDTO.Date,
                    HoursWorked = timetrackingDTO.HoursWorked,                   
                    UserId = timetrackingDTO.UserID
                };
                _context.Timetrackings.Add(timetracking);

                // Create the track to access its ID
                await _context.SaveChangesAsync();
                                                 
                
                
                return Ok();
            }
            return BadRequest(ModelState);
        }

        //[AllowAnonymous]
        //[HttpGet("ReadExercises")]
        //public async Task<IActionResult> ReadExercises()
        //{
        //    var exercises = await _context.Exercises.ToListAsync();

        //    var exerciseDTOs = exercises.Select(e => new ExerciseDTO
        //    {
        //        ExerciseID = e.ExerciseID,
        //        ExerciseIllustrationPath = e.ExerciseIllustrationPath,
        //        ExercisePositionX = e.ExercisePositionX,
        //        ExercisePositionY = e.ExercisePositionY
        //    });
        //    return Ok(exerciseDTOs);
        //}

        //[Authorize(Roles = "Admin, User")]
        //[HttpGet("ReadTrack")]
        //public async Task<IActionResult> ReadTrack(int? trackID)
        //{
        //    if (ModelState.IsValid && trackID != null)
        //    {
        //        var trackExerciseDTOs = await _context.TrackExerciseDTOS.Where(te => te.ForeignTrackID == trackID).ToListAsync();

        //        return Ok(trackExerciseDTOs);
        //    }
        //    return BadRequest(ModelState);
        //}

        //[Authorize(Roles = "Admin, User")]
        //[HttpGet("GetUserTracks")]
        //public async Task<IActionResult> GetUserTracks(string userID)
        //{
        //    if(userID == null)
        //    {
        //        return Unauthorized();  
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var allUserTracks = await _context.Tracks.Where(track => track.UserID == userID).ToListAsync();

        //        var allUserTracksDTO = allUserTracks.Select(u => new TimetrackingDTO
        //        {
        //            TrackID = u.TrackID,
        //            TrackName = u.TrackName,
        //            TrackDate = u.TrackDate,
        //        });
        //        return Ok(allUserTracksDTO);
        //    }
        //    return BadRequest(ModelState);
        //}

       
        //[Authorize(Roles = "Admin")]
        //[HttpGet("GetAllTracks")]
        //public async Task<IActionResult> GetAllTracks()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var allTracks = await _context.Tracks.ToListAsync();

        //        var allTracksDTO = allTracks.Select(u => new TimetrackingDTO
        //        {
        //            TrackID = u.TrackID,
        //            TrackName = u.TrackName,
        //            TrackDate = u.TrackDate,
        //        });
        //        return Ok(allTracksDTO);
        //    }
        //    return BadRequest(ModelState);
        //}


        //[Authorize(Roles = "Admin, User")]
        //[HttpPut("UpdateTrack")]
        //public async Task<IActionResult> UpdateTrack([FromBody] TrackExerciseViewModelDTO viewModel)
        //{
        //    if (viewModel == null || viewModel.Track == null)
        //    {
        //        return BadRequest("Invalid data.");
        //    }

        //    // Fetch the existing track from the database
        //    var trackToUpdate = await _context.Tracks
        //        .Include(t => t.TrackExercises) // Make sure to include related data
        //        .FirstOrDefaultAsync(t => t.TrackID == viewModel.Track.TrackID);

        //    if (trackToUpdate == null)
        //    {
        //        return NotFound($"Track with ID {viewModel.Track.TrackID} not found.");
        //    }

        //    // Update track properties
        //    trackToUpdate.TrackName = viewModel.Track.TrackName;
        //    trackToUpdate.TrackDate = viewModel.Track.TrackDate;

        //    // Update or add exercises
        //    foreach (var exerciseViewModel in viewModel.Exercises)
        //    {
        //        var trackExercise = trackToUpdate.TrackExercises
        //            .FirstOrDefault(te => te.ForeignExerciseID == exerciseViewModel.ExerciseID);

        //        if (trackExercise != null)
        //        {
        //            // Update existing trackExercise
        //            trackExercise.TrackExercisePositionX = exerciseViewModel.ExercisePositionX;
        //            trackExercise.TrackExercisePositionY = exerciseViewModel.ExercisePositionY;
        //        }
        //        else if (await _context.Exercises.AnyAsync(e => e.ExerciseID == exerciseViewModel.ExerciseID))
        //        {
        //            // Add new trackExercise if it doesn't exist and the exercise is valid
        //            trackToUpdate.TrackExercises.Add(new TrackExercise
        //            {
        //                // This sets the relationship
        //                ForeignTrackID = trackToUpdate.TrackID,
        //                ForeignExerciseID = exerciseViewModel.ExerciseID,
        //                TrackExercisePositionX = exerciseViewModel.ExercisePositionX,
        //                TrackExercisePositionY = exerciseViewModel.ExercisePositionY
        //            });
        //        }
        //    }
        //    // Save changes to the database
        //    await _context.SaveChangesAsync();
        //    return Ok();
        //}

        //[Authorize(Roles = "Admin, User")]
        //[HttpDelete("DeleteTrack/{id}")]
        //public async Task<IActionResult> DeleteTrack(int id)
        //{
        //    var track = await _context.Tracks
        //        .Include(t => t.TrackExercises)  // If you store related data that also needs to be deleted
        //        .FirstOrDefaultAsync(t => t.TrackID == id);

        //    if (track == null)
        //    {
        //        return NotFound($"Track with ID {id} not found.");
        //    }

        //    _context.Tracks.Remove(track);
        //    await _context.SaveChangesAsync();
        //    return Ok($"Track with ID {id} has been deleted.");
        //}
    }
}

