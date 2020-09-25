using GardenHub.Repository.Account;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GardenHub.Services.Authenticate
{
    public class AuthenticateService
    {
        private AccountRepository Repository { get; set; }

        private IConfiguration Configuration { get; set; }

        public AuthenticateService(AccountRepository repository, IConfiguration configuration)
        {
            this.Repository = repository;
            this.Configuration = configuration;
        }

        public string AuthenticateUser(string email, string password)
        {
            var account = this.Repository.GetAll().Where(x => x.Email == email).FirstOrDefault();

            if (account == null)
                return null;

            if (account.Password != password)
                return null;

            return CreateToken(account);
        }

        private string CreateToken(Domain.Account.Account account)
        {
            var key = Encoding.UTF8.GetBytes(this.Configuration["Token:Secret"]);

            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, account.Name));
            claims.Add(new Claim(ClaimTypes.Email, account.Email));

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Audience = "LOGIN-API",
                Issuer = "LOGIN-API"
            };

            var securityToken = tokenHandler.CreateToken(tokenDescription);

            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
