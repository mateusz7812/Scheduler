using HotChocolate;
using HotChocolate.Types;
using System.Collections.Generic;

namespace SchedulerWebApplication.Mutations
{
    public class TaskInput
    {
        public string InputType { get; set; }
        public string OutputType { get; set; }
        public string Name { get; set; }
        
        public string Command { get; set; }

        public List<KeyValuePair<string, string>> DefaultEnvironmentVariables { get; set; }
    }
}