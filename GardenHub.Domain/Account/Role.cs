using System;
using System.Collections.Generic;

namespace GardenHub.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Domain.Account.Account> Accounts { get; set; }
    }
}
