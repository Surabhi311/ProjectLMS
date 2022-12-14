using System;
using System.Collections.Generic;

#nullable disable

namespace ProjectLMS.Models
{
    public partial class LendRequest
    {
        public int LendId { get; set; }
        public string LendStatus { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public double? FineAmount { get; set; }

        public virtual Book Book { get; set; }
        public virtual Account User { get; set; }
    }
}
