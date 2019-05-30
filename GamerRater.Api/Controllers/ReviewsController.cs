using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
    public class ReviewsController : ControllerBase
    {
        private readonly DataContext _context;

        public ReviewsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetRatings()
        {
            try
            {
                return await _context.Ratings.ToListAsync();
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
            
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            
            try
            {
                var review = await _context.Ratings.FindAsync(id);

                if (review == null)
                {
                    return NotFound();
                }

                return review;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // PUT: api/Reviews/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            

            try
            {
                if (id != review.Id)
                {
                    return BadRequest();
                }

                _context.Entry(review).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok();
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // POST: api/Reviews
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            
            try
            {
                _context.Entry(review.User).State = EntityState.Modified;
                _context.Ratings.Add(review);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetReview", new { id = review.Id }, review);
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            try
            {
                var review = await _context.Ratings.FindAsync(id);
                if (review == null)
                {
                    return NotFound();
                }

                _context.Ratings.Remove(review);
                await _context.SaveChangesAsync();

                return review;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        private bool ReviewExists(int id)
        {
            return _context.Ratings.Any(e => e.Id == id);
        }
    }
}
