using System;
using System.ComponentModel.DataAnnotations;

namespace GardenHub.Web.ViewModel.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        public String Email { get; set; }
        [Display(Name = "Password")]
        public String Password { get; set; }
    }
}
