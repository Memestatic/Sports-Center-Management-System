namespace ProjectIO.model
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("WorkerFunctions")] 
public class WorkerFunction
{
    [Key] 
    public int functionId { get; set; }

    [Required] 
    [MaxLength(50)] 
    public string functionName { get; set; }

}
