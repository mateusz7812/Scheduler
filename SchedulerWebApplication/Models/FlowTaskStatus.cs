using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerWebApplication.Models
{
    public class FlowTaskStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int FlowRunId { get; set; }
        public int FlowTaskId { get; set; }
        public FlowTaskStatusCode StatusCode { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        public long Date { get; set; }
    }
}
