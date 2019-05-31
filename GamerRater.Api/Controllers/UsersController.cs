using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                //var user = await _context.Users.Where(x => x.Id == id).Include(x => x.Reviews).FirstAsync();
                var user = await _context.Users.FindAsync(id);
                //Fetch reviews and userGroups
                try
                {
                    _context.Users
                        .Where(x => x.Id == id)
                        .Include(x => x.Reviews)
                        .Load();

                    _context.Users
                        .Where(x => x.Id == id)
                        .Include(x => x.UserGroups)
                        .Load();
                }
                catch (InvalidOperationException ex)
                {
                    Debug.WriteLine("ERROR Message: Something happened while binding reviews/user groups to users..." +
                                    ex);
                    return NoContent();
                }

                if (user == null) return NotFound();
                return user;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // GET: api/Users/username
        [HttpGet("Username/{username}")]
        public async Task<ActionResult<User>> GetUserWithUsername(string username)
        {
            try
            {
                User user;
                try
                {
                    user = await _context.Users.Where(x => x.Username == username).FirstAsync();
                }
                catch (InvalidOperationException e)
                {
                    //User was not found.
                    return NotFound();
                }

                return Ok(user);
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // GET: api/Users/42/UserGroup/24
        [HttpGet("{userId}/UserGroup/{userGroupId}")]
        public async Task<IActionResult> GetUserHasUserGroup([FromRoute] int userId, [FromRoute] int userGroupId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (UserHasUserGroupExists(userId, userGroupId)) return NoContent();

                var userGroup = await _context.UserGroups.FindAsync(userGroupId);

                if (userGroup == null) return NotFound();

                return Ok(userGroup);
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            try
            {
                if (id != user.Id) return BadRequest();

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id)) return NotFound();
                    throw;
                }

                return NoContent();
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        //Place user in group
        // PUT: api/Users/1/UserGroups/2
        [HttpPut("{userId}/UserGroups/{userGroupId}")]
        public async Task<IActionResult> AddUserGroupToUser([FromRoute] int userId, [FromRoute] int userGroupId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (!UserExists(userId)) return NoContent();

                if (UserHasUserGroupExists(userId, userGroupId)) return NoContent();

                var userHasUserGroup = new UserHasUserGroup {UserId = userId, UserGroupId = userGroupId};
                _context.UserHasUserGroups.Add(userHasUserGroup);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUserHasUserGroup", new {userId, userGroupId}, userHasUserGroup);
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                try
                {
                    var userGroup = _context.UserGroups.Single(x => x.Group.Equals("User"));
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    var userHasUserGroup = new UserHasUserGroup {UserId = user.Id, UserGroupId = userGroup.Id};
                    _context.UserHasUserGroups.Add(userHasUserGroup);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetUser", new {id = user.Id}, user);
                }
                catch (InvalidOperationException)
                {
                    Debug.WriteLine("ERROR: Duplicate user groups. Delete all values in user groups table and try again");
                    return NoContent();
                }
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null) return NotFound();

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        /// <summary>  Check if there exists a relation between user and user group</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userGroupId">The user group identifier.</param>
        /// <returns></returns>
        private bool UserHasUserGroupExists(int userId, int userGroupId)
        {
            return _context.UserHasUserGroups.Any(uug => uug.UserId == userId && uug.UserGroupId == userGroupId);
        }
    }
}