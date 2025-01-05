using System;
using System.ComponentModel.DataAnnotations;

namespace WealthWatch.Models
{
    public class Transactions
    {
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; } 

        public string Type { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;


        public string Amount { get; set; } = string.Empty;
        public int Pay { get; set; } = 0;

        public string Note { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Today;

        public DateTime? DueDate { get; set; } = null;
        public string Tag { get; set; } = string.Empty;

        public string Status { get; set; } = "Incomplete";
    }
}
