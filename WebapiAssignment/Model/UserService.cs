using System.Collections.Generic;
using System.Linq;
using Assignment.Framework.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Assignment.Framework.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Assignment.Framework.Service
{
    public interface IUserService
    {
        public AuthenticateResponse Authenticate(AuthenticateRequest model);
        public IEnumerable<RegisteredUser> GetAll(List<RegisteredUser> registeredUsers);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, DataContext context)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _context.RegisteredUsers.SingleOrDefault(x => x.UserId == model.Username && x.UserPassword == model.Password);
            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName.ToString()),
                    new Claim(ClaimTypes.Surname, user.LastName.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.IsPersistent, user.IsActive.ToString()),
                    new Claim(ClaimTypes.Role, user.Roles)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string Token = tokenHandler.WriteToken(token);

            return new AuthenticateResponse(Token);
        }

        public IEnumerable<RegisteredUser> GetAll(List<RegisteredUser> registeredUsers)
        {
            return _context.RegisteredUsers.Select(s => new RegisteredUser
            {
                UserId = s.UserId,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                IsActive = s.IsActive,
                Roles = s.Roles
            }).ToList();
        }
    }
}
