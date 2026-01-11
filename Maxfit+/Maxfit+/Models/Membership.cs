using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maxfit_.Models
{
    public partial class Membership
    {
        [Key]
        public int MembershipId { get; set; }

        public int MemberId { get; set; }

        public int MembershipTypeId { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // --- Navigation Properties ---

        [ForeignKey("MemberId")]
        public virtual Member? Member { get; set; }

        [ForeignKey("MembershipTypeId")]
        public virtual MembershipType? MembershipType { get; set; }
    }
}