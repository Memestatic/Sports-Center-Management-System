
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Users")]
    public class User : Person
    {
        [Key]
        public int userId { get; set; }
    }
}


