namespace ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Users")]
public class User
{
    [Key]
    public int userId { get; set; }

    [Required]
    [MaxLength(50)]
    public string name { get; set; }

    [Required]
    [MaxLength(50)]
    public string surname { get; set; }

    [Required]
    public Gender gender { get; set; }

    [Required]
    [Phone]
    public string phone { get; set; }


}
