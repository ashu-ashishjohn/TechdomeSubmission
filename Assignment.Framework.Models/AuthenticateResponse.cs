using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment.Framework.Models
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public AuthenticateResponse(string token)
        {
            Token = token;
        }
    }
}