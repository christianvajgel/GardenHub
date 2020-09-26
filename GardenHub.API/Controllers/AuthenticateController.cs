using System;
using System.Threading.Tasks;
using GardenHub.API.ViewModel.Account;
using GardenHub.Services.Authenticate;
using Microsoft.AspNetCore.Mvc;

namespace GardenHub.API.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private AuthenticateService AuthenticateService { get; set; }

        public AuthenticateController(AuthenticateService service)
        {
            this.AuthenticateService = service;
        }

        [Route("Token")]
        [HttpPost]
        [RequireHttps]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return await Task.FromResult(BadRequest(ModelState));

            var token = this.AuthenticateService.AuthenticateUser(loginViewModel.Email, loginViewModel.Password);

            if (String.IsNullOrWhiteSpace(token))
            {
                return await Task.FromResult(BadRequest("Invalid credentials."));
            }

            return Ok(new
            {
                Token = token
            });
        }
    }
}
