using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GardenHub.Domain.Account
{
    public class Account
    {
        public Guid Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Birthday")]
        public DateTime DtBirthday { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Password")]
        public string Password { get; set; }
        public Role Role { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }

        // Reference POST to USER
        public virtual IList<Domain.Post.Post> Posts { get; set; }

        //public List<Domain.Post.Post> Posts { get; set; }
    }
}
