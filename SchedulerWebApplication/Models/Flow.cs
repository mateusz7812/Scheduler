using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotChocolate;

namespace SchedulerWebApplication.Models
{
    public class Flow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [GraphQLIgnore]
        public int PersonId { get; set; }
        public int? FlowTaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        [GraphQLIgnore]
        public Person Person { get; set; }
        [GraphQLIgnore]
        public FlowTask FlowTask { get; set; }

        [GraphQLIgnore]
        public ICollection<FlowRun> FlowRuns { get; set; }
    }
}