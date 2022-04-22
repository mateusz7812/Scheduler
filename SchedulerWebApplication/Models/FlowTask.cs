using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerWebApplication.Models
{
    public class FlowTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TaskId { get; set; }
        
        public Flow Flow { get; set; }
        public Task Task { get; set; }
        
        public virtual ICollection<StartingUp> Predecessors { get; set; }
        public virtual ICollection<StartingUp> Successors { get; set; }

    }
}