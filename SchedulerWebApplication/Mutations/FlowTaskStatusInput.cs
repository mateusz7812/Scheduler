using SchedulerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerWebApplication.Mutations
{
    public class FlowTaskStatusInput
    {
        public int FlowRunId { get; set; }
        public int FlowTaskId { get; set; }
        public FlowTaskStatusCode StatusCode { get; set; }

        public string Description { get; set; }
        public long Date { get; set; }
    }
}
