using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.core.entities
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Relacionamento com o usuário
        public int UserId { get; set; }
        public User User { get; set; }

        // Um projeto tem várias tarefas
        public List<Job> Jobs { get; set; }
    }
}
