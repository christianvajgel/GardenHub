using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GardenHub.API.ViewModel.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        public String UserName { get; set; }
        [Display(Name = "Password")]
        public String Password { get; set; }
    }
}
