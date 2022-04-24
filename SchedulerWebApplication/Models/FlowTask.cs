using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HotChocolate;

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

        public IEnumerable<int> SuccessorsIds => Successors.Select(t => t.SuccessorId);

        [GraphQLIgnore]
        public virtual ICollection<StartingUp> Predecessors { get; set; }
        [GraphQLIgnore]
        public virtual ICollection<StartingUp> Successors { get; set; }

    }
}