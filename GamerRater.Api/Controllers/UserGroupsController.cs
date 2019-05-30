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
    public class UserGroupsController : ControllerBase
    {
        private readonly DataContext _context;

        public UserGroupsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/UserGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGroup>>> GetUserGroups()
        {
            try
            {
                return await _context.UserGroups.ToListAsync();
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // GET: api/UserGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGroup>> GetUserGroup(int id)
        {
            try
            {
                var userGroup = await _context.UserGroups.FindAsync(id);

                if (userGroup == null)
                {
                    return NotFound();
                }

                return userGroup;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        [HttpGet("Group/{groupName}")]
        public async Task<ActionResult<UserGroup>> GetUserGroupWithGroupName(string groupName)
        {
            try
            {
                UserGroup userGroup = null;
                try
                {
                    userGroup = await _context.UserGroups.Where(x => x.Group == groupName).FirstAsync();
                }
                catch (InvalidOperationException e)
                {
                    //UserGroup was not found.
                    return NoContent();
                }

                return userGroup;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // PUT: api/UserGroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserGroup(int id, UserGroup userGroup)
        {
            try
            {
                if (id != userGroup.Id)
                {
                    return BadRequest();
                }

                _context.Entry(userGroup).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserGroupExists(id))
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
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // POST: api/UserGroups
        [HttpPost]
        public async Task<ActionResult<UserGroup>> PostUserGroup(UserGroup userGroup)
        {
            try
            {
                _context.UserGroups.Add(userGroup);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUserGroup", new { id = userGroup.Id }, userGroup);
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        // DELETE: api/UserGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserGroup>> DeleteUserGroup(int id)
        {
            try
            {
                var userGroup = await _context.UserGroups.FindAsync(id);
                if (userGroup == null)
                {
                    return NotFound();
                }

                _context.UserGroups.Remove(userGroup);
                await _context.SaveChangesAsync();

                return userGroup;
            }
            catch (SqlException)
            {
                return StatusCode(503, null);
            }
        }

        private bool UserGroupExists(int id)
        {
            return _context.UserGroups.Any(e => e.Id == id);
        }
    }
}
