using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GamerRater.DataAccess;
using GamerRater.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GamerRater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly DataContext _context;

        public PlatformsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Platforms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Platform>>> GetPlatforms()
        {
            try
            {
                return await _context.Platforms.ToListAsync();
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // GET: api/Platforms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Platform>> GetPlatform(int id)
        {
            try
            {
                var platform = await _context.Platforms.FindAsync(id);

                if (platform == null) return NotFound();

                return platform;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // PUT: api/Platforms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlatform(int id, Platform platform)
        {
            

            try
            {
                if (id != platform.Id) return BadRequest();

                _context.Entry(platform).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatformExists(id))
                        return NotFound();
                    throw;
                }

                return NoContent();
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // POST: api/Platforms
        [HttpPost]
        public async Task<ActionResult<Platform>> PostPlatform(Platform platform)
        {
            

            try
            {
                _context.Platforms.Add(platform);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPlatform", new {id = platform.Id}, platform);
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // DELETE: api/Platforms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Platform>> DeletePlatform(int id)
        {
            try
            {
                var platform = await _context.Platforms.FindAsync(id);
                if (platform == null) return NotFound();

                _context.Platforms.Remove(platform);
                await _context.SaveChangesAsync();

                return platform;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
            
        }

        private bool PlatformExists(int id)
        {
            return _context.Platforms.Any(e => e.Id == id);
        }
    }
}