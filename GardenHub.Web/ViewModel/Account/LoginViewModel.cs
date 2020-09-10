using System;
using System.ComponentModel.DataAnnotations;

namespace GardenHub.Web.ViewModel.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        public String UserName { get; set; }
        [Display(Name = "Password")]
        public String Password { get; set; }
    }
}
