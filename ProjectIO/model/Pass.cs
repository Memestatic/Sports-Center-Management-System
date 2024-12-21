﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Passes")]
    public class Pass
    {
        [Key]
        public int passId { get; set; }

        [ForeignKey("passTypeId")]
        public PassType passType { get; set; }

        [ForeignKey("userId")]
        public User user { get; set; }

        [Required]
        public int passEntriesLeft { get; set; }

    }
}



