using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectLMS.Models
{
    public partial class Account
    {
        public Account()
        {
            LendRequests = new HashSet<LendRequest>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public virtual ICollection<LendRequest> LendRequests { get; set; }
    }
}
