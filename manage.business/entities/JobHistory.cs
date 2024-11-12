using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace manage.core.entities;

public class JobHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int JobId { get; set; }
    public string ChangeDescription { get; set; }
    public DateTime ChangeDate { get; set; }
    public int UserId { get; set; }
}