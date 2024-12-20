namespace ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("LoginCredentials")]
public class LoginCredential
{
    [Key]
    public int loginId { get; set; }

    [Required]
    [EmailAddress]
    public string email { get; set; }

    [Required]
    [MaxLength(50)]
    public string password { get; set; }


} 
