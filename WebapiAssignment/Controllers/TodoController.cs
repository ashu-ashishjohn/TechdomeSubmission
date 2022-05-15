using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Assignment.Framework.Helpers;
using Assignment.Framework.Models;
using Assignment.Framework.Service;

namespace WebapiAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private DataContext _context;
        private IUserService _userService;

        public TodoController(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/Todo
        [HttpGet]
        [Route("GetRegisteredUsers")]
        public IActionResult GetRegisteredUsers()
        {
            return Ok(_userService.GetAll(_context.RegisteredUsers.ToList()));
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        [Route("GetRegisteredUserID")]
        public IActionResult GetRegisteredUserID(string UserId)
        {
            if (User.IsInRole(Role.Admin))
            {
                var registeredUser = _context.RegisteredUsers.ToList();
                var UserById = registeredUser.Where(x => x.UserId == UserId);
                if (UserById == null)
                {
                    return Ok("User Id Not Found");
                }
                else
                {
                    return Ok(UserById);
                }
            }
            else
            {
                return Unauthorized();
            }

        }

        // PUT: api/Todo/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Route("PutRegisteredUser")]
        public IActionResult PutRegisteredUser(RegisteredUser registeredUser)
        {
            if (User.IsInRole(Role.Admin))
            {
                var IdUser = _context.RegisteredUsers.Where(x => x.UserId == registeredUser.UserId).ToList();
                if (IdUser != null)
                {
                    _context.RegisteredUsers.Add(new RegisteredUser
                    {
                        UserId = registeredUser.UserId,
                        FirstName = registeredUser.FirstName,
                        LastName = registeredUser.LastName,
                        Email = registeredUser.Email,
                        IsActive = registeredUser.IsActive,
                        Roles = registeredUser.Roles
                    });
                    _context.SaveChanges();
                    return Ok(_context.RegisteredUsers.Where(x => x.UserId == registeredUser.UserId).ToList());
                }
                return Ok("User Id Not Found");
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST: api/Todo
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("PostRegisteredUser")]
        public IActionResult PostRegisteredUser(RegisteredUser registeredUser)
        {
            if (User.IsInRole(Role.Admin))
            {
                _context.RegisteredUsers.Add(new RegisteredUser
                {
                    UserId = registeredUser.UserId,
                    UserPassword = registeredUser.UserPassword,
                    FirstName = registeredUser.FirstName,
                    LastName = registeredUser.LastName,
                    Email = registeredUser.Email,
                    IsActive = registeredUser.IsActive,
                    Roles = registeredUser.Roles
                });
                _context.SaveChanges();
                return Ok(_context.RegisteredUsers.ToList());
            }
            else
            {
                return Unauthorized();
            }
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        [Route("DeleteRegisteredUser")]
        public IActionResult DeleteRegisteredUser(string UserId)
        {
            if (User.IsInRole(Role.Admin))
            {
                var itemToRemove = _context.RegisteredUsers.SingleOrDefault(r => r.UserId == UserId);
                if (itemToRemove != null)
                    _context.RegisteredUsers.Remove(itemToRemove);
                _context.SaveChanges();
                return Ok(_context.RegisteredUsers.ToList());
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
