using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HotChocolate;

namespace SchedulerWebApplication.Models
{
    public class Executor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [GraphQLIgnore]
        public int PersonId { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        
        public virtual Person Person { get; set; }
        
        [NotMapped]
        public ExecutorStatus Status
        {
            get
            {
                return Statuses?.OrderByDescending(t => t.Date).FirstOrDefault() ??
                       new ExecutorStatus {StatusCode = ExecutorStatusCode.Offline};
            }
            set
            {
                throw new NotSupportedException();    
            }
        }

        [GraphQLIgnore] 
        public ICollection<ExecutorStatus> Statuses { get; set; } = new List<ExecutorStatus>();

        [GraphQLIgnore]
        public ICollection<FlowRun> FlowRuns { get; set; }
    }
}