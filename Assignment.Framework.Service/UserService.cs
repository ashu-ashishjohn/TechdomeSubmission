using System.Collections.Generic;
using System.Linq;
using Assignment.Framework.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Assignment.Framework.Models;
using Assignment.Framework.Models.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Configuration;


namespace Assignment.Framework.Service
{
    public interface IUserService1
    {
        public AuthenticateResponse Authenticate(AuthenticateRequest model);
        public IEnumerable<RegisteredUser> GetAll();
        public RegisteredUser GetById(int id);
    }


    public class UserService : IUserService1
    {
        private DataContext _context;
        private AppSettings _appSettings;
        private IConfiguration _configuration;

        public UserService(DataContext context,AppSettings appSettings,IConfiguration configuration)
        {
            _context = context;
            _appSettings = appSettings;
            _configuration = configuration;
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
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Roles)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string Token = tokenHandler.WriteToken(token);

            return new AuthenticateResponse(user,Token);
        }

        public IEnumerable<RegisteredUser> GetAll()
        {
           return _context.RegisteredUsers.ToList();
        }

        public RegisteredUser GetById(int id)
        {
            var user = _context.RegisteredUsers.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
