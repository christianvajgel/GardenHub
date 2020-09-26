using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GardenHub.API.ViewModel.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        public String Email { get; set; }
        [Display(Name = "Password")]
        public String Password { get; set; }
    }
}
