using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerWebApplication.Models
{
    public enum FlowTaskStatusCode
    {
        Wait = 0,
        Cancelled = 10,
        Processing = 20,
        Done = 30,
        Error = 40
    }
}
