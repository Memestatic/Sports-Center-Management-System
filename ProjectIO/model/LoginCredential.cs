namespace ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("LoginCredentials")]
public class LoginCredential
{
    [Key]
    public int loginId { get; set; }

    [Required]
    [MaxLength(50)]
    public string username { get; set; }

    [Required]
    [MaxLength(50)]
    public string password { get; set; }


} 
