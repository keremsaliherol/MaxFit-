using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maxfit_.Models
{
    public partial class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int MemberId { get; set; }

        public int? MembershipId { get; set; } // Links to MembershipTypes table (column exists in DB)

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = null!; // Cash, Credit Card, Bank Transfer, Online

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Completed"; // Completed, Pending, Failed, Refunded

        [StringLength(500)]
        public string? Notes { get; set; }

        [StringLength(100)]
        public string? TransactionId { get; set; } // For online payments

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("MemberId")]
        public virtual Member? Member { get; set; }

        // Note: MembershipId links to MembershipTypes table (not Memberships)
        [ForeignKey("MembershipId")]
        public virtual MembershipType? MembershipType { get; set; }
    }
}
