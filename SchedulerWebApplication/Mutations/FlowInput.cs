﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerWebApplication.Mutations
{
    public class FlowInput
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
