using HotChocolate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerWebApplication.Models
{
    public class FlowRun
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long RunDate { get; set; }

        public int FlowId { get; set; }
        
        public int ExecutorId { get; set; }

        [NotMapped]
        public FlowTaskStatusCode Status
        {
            get
            {
                return Statuses is null ? FlowTaskStatusCode.Wait 
                    : Statuses.Any(s => s.StatusCode == FlowTaskStatusCode.Error) ? FlowTaskStatusCode.Error
                    : Statuses.All(s => s.StatusCode == FlowTaskStatusCode.Done) ? FlowTaskStatusCode.Done 
                    : FlowTaskStatusCode.Processing;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        [GraphQLIgnore]
        public Flow Flow { get; set; }

        [GraphQLIgnore]
        public Executor Executor { get; set; }

        [GraphQLIgnore]
        public ICollection<FlowTaskStatus> Statuses { get; set; } = new List<FlowTaskStatus>();
    }
}
