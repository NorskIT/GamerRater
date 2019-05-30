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
    public class GameRootsController : ControllerBase
    {
        private readonly DataContext _context;

        public GameRootsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/GameRoots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameRoot>>> GetGames()
        {
            try
            {
                return await _context.Games.ToListAsync();
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // GET: api/GameRoots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameRoot>> GetGameRoot(int id)
        {
            try
            {
                var gameRoot = await _context.Games.FindAsync(id);

                if (gameRoot == null) return NotFound();

                await _context.Entry(gameRoot).Collection(r => r.Reviews).LoadAsync();
                await _context.Entry(gameRoot).Reference(x => x.GameCover).LoadAsync();

                return gameRoot;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // PUT: api/GameRoots/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameRoot(int id, GameRoot gameRoot)
        {
            try
            {
                if (id != gameRoot.Id) return BadRequest();

                _context.Entry(gameRoot).State = EntityState.Unchanged;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameRootExists(id))
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

        // POST: api/GameRoots
        [HttpPost]
        public async Task<ActionResult<GameRoot>> PostGameRoot(GameRoot gameRoot)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                try
                {
                    gameRoot.PlatformList = null;
                    _context.Games.Add(gameRoot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    return BadRequest();
                }


                return CreatedAtAction("GetGameRoot", new {id = gameRoot.Id}, gameRoot);
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // DELETE: api/GameRoots/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameRoot>> DeleteGameRoot(int id)
        {
            try
            {
                var gameRoot = await _context.Games.FindAsync(id);
                if (gameRoot == null) return NotFound();

                _context.Games.Remove(gameRoot);
                await _context.SaveChangesAsync();

                return gameRoot;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        private bool GameRootExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}