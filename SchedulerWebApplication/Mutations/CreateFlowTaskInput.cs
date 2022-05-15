using System.Collections.Generic;
using HotChocolate;
using HotChocolate.Types;

namespace SchedulerWebApplication.Mutations
{
    public class CreateFlowTaskInput
    {
        public int TaskId { get; set; }
        //public IEnumerable<int> SuccessorsIds;
        
        //[GraphQLType(typeof(AnyType))]
        //public Dictionary<string, string> EnvironmentVariables { get; set; }
    }
}