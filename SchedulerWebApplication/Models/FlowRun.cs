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

        [GraphQLIgnore]
        public Flow Flow { get; set; }

        [GraphQLIgnore]
        public Executor Executor { get; set; }
    }
}
