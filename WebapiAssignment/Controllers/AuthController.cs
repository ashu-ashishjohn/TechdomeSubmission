using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment.Framework.Helpers;
using Assignment.Framework.Models;
using Assignment.Framework.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebapiAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private DataContext _context;

        public AuthController(IUserService userService, DataContext context)
        {
            _userService = userService;
            _context = context;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var user = _userService.Authenticate(model);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(RegisteredUser registeredUser)
        {
            _context.RegisteredUsers.Local.Add(new RegisteredUser
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
            return Ok("Resgistered");
        }
    }
}
