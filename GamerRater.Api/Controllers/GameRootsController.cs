using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GamerRater.DataAccess;
using GamerRater.Model;

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
            return await _context.Games.ToListAsync();
        }

        // GET: api/GameRoots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameRoot>> GetGameRoot(int id)
        {
            var gameRoot = await _context.Games.FindAsync(id);
            
            if (gameRoot == null)
            {
                return NotFound();
            }

            await _context.Entry(gameRoot).Collection(r => r.Reviews).LoadAsync();
            await _context.Entry(gameRoot).Reference(x => x.GameCover).LoadAsync();
            
            return gameRoot;
        }

        // PUT: api/GameRoots/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameRoot(int id, GameRoot gameRoot)
        {
            if (id != gameRoot.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameRoot).State = EntityState.Unchanged;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameRootExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GameRoots
        [HttpPost]
        public async Task<ActionResult<GameRoot>> PostGameRoot(GameRoot gameRoot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                gameRoot.PlatformList = null;
                _context.Games.Add(gameRoot);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            

            return CreatedAtAction("GetGameRoot", new { id = gameRoot.Id }, gameRoot);
        }

        // DELETE: api/GameRoots/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameRoot>> DeleteGameRoot(int id)
        {
            var gameRoot = await _context.Games.FindAsync(id);
            if (gameRoot == null)
            {
                return NotFound();
            }

            _context.Games.Remove(gameRoot);
            await _context.SaveChangesAsync();

            return gameRoot;
        }

        private bool GameRootExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
