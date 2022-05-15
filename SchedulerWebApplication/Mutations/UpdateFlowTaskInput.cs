using System.Collections.Generic;
using GraphQL.Types;
using HotChocolate;
using HotChocolate.Types;

namespace SchedulerWebApplication.Mutations
{
    public class UpdateFlowTaskInput
    {
        public int Id { get; set; }
        public int? TaskId { get; set; }
        public List<KeyValuePair<string, string>> EnvironmentVariables { get; set; }
        public List<int> SuccessorsIds { get; set; }
    }
}