using manage.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.aplication.dto
{
    public class JobDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PriorityJob Priority { get; set; }
        public StatusJob Status { get; set; }
        public string Comment { get; set; }
    }
   
    
}
