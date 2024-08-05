using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrackingServerAPI.Data;
using TimeTrackingServerAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTrackingServerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeRegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TimeRegistrationController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostTimeRegistration([FromBody] TimeRegistrationDTO timeRegistrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var timeRegistration = new TimeRegistration
            {
                Date = timeRegistrationDTO.Date,
                HoursWorked = timeRegistrationDTO.HoursWorked,
                UserId = userId
            };

            _context.TimeRegistrations.Add(timeRegistration);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTimeRegistrationById), new { id = timeRegistration.Id }, timeRegistration);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimeRegistrationById(int id)
        {
            var registration = await _context.TimeRegistrations.FindAsync(id);

            if (registration == null)
            {
                return NotFound();
            }

            return Ok(registration);
        }

        // GET: TimeRegistration/AllHoursWorked
        [HttpGet("AllHoursWorked")]
        public async Task<IActionResult> GetAllHoursWorked()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var timeRegistrations = await _context.TimeRegistrations
                .Where(tr => tr.UserId == userId)
                .ToListAsync();

            return Ok(timeRegistrations);
        }
    }
}

