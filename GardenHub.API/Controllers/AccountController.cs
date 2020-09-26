using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GardenHub.Domain.Account;
using GardenHub.Services.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GardenHub.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService AccountServices { get; set; }

        public AccountController(IAccountService accountServices)
        {
            this.AccountServices = accountServices;
        }

        // GET: api/<AccountController>
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return this.AccountServices.GetAll();
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public Account Get(Guid id)
        {
            return this.AccountServices.GetAll().Where(x => x.Id == id).FirstOrDefault();
        }

        // GET: One account by email
        [HttpGet("getbyemail/{email}")]
        public Account Get([FromRoute] string email)
        {
            return this.AccountServices.GetAll().Where(x => x.Email.Replace(" ","").ToLower() == email.Replace(" ", "").ToLower()).FirstOrDefault();
        }
    }
}
