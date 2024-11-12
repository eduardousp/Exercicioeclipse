using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace manage.aplication.commands
{
    public class AddJobHistoryCommand
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string ChangeDescription { get; set; }
        public DateTime ChangeDate { get; set; }
        public int UserId { get; set; }
   
    }
}
