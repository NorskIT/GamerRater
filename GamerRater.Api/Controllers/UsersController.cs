using System;
using System.Collections.Generic;
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
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
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
            catch (Exception ex)
            {
                // TODO: fdsf
            }

            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // GET: api/Users/username
        [HttpGet("Username/{username}")]
        public async Task<ActionResult<User>> GetUserWithUsername(string username)
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

            return user;
        }

        // GET: api/Users/42/UserGroup/24
        [HttpGet("{userId}/UserGroup/{userGroupId}")]
        public async Task<IActionResult> GetUserHasUserGroup([FromRoute] int userId, [FromRoute] int userGroupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (UserHasUserGroupExists(userId, userGroupId))
            {
                return NoContent();
            }

            var userGroup = await _context.UserGroups.FindAsync(userGroupId);

            if (userGroup == null)
            {
                return NotFound();
            }

            return Ok(userGroup);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        //Place user in group
        // PUT: api/Users/1/UserGroups/2
        [HttpPut("{userId}/UserGroups/{userGroupId}")]
        public async Task<IActionResult> AddUserGroupToUser([FromRoute] int userId, [FromRoute] int userGroupId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!UserExists(userId))
            {
                return NoContent();
            }

            if (UserHasUserGroupExists(userId, userGroupId))
            {
                return NoContent();
            }

            var userHasUserGroup = new UserHasUserGroup() { UserId = userId, UserGroupId = userGroupId };
            _context.UserHasUserGroups.Add(userHasUserGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserHasUserGroup", new { userId, userGroupId }, userHasUserGroup);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //TODO: Comments
                UserGroup userGroup;
                try
                {
                    userGroup = _context.UserGroups.Single(x => x.Group.Equals("User"));
                }
                catch (InvalidOperationException)
                {
                    //More than one User-group. Do not continue before duplicate is removed.
                    throw;
                }
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var userHasUserGroup = new UserHasUserGroup() { UserId = user.Id, UserGroupId =userGroup.Id };
                _context.UserHasUserGroups.Add(userHasUserGroup);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (InvalidOperationException)
            {
                //TODO: this
                return NoContent();
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool UserHasUserGroupExists(int userId, int userGroupId)
        {
            return _context.UserHasUserGroups.Any(uug => uug.UserId == userId && uug.UserGroupId == userGroupId);
        }
    }
}
