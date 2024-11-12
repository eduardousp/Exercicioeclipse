using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.core.entities
{
    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Validated { get; set; }
        public StatusJob Status { get; set; }
        public PriorityJob Priority { get; set; } // Baixa, Média, Alta
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string Comment { get; set; }

    }
    public enum StatusJob
    {
        Pendente,
        EmAndamento,
        Concluida
    }
    public enum PriorityJob
    {
        Baixa, Média, Alta
    }
}
