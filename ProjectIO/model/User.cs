﻿
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Users")]
    public class User : Person
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public bool MarketingConsent { get; set; } = false;

        [Required]
        public bool IsActive { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public ICollection<VerificationToken> VerificationTokens { get; set; } = new List<VerificationToken>();

    }
}


