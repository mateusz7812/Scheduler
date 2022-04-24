using HotChocolate;
using HotChocolate.Types;
using SchedulerWebApplication.Models;

namespace SchedulerWebApplication.Subscriptions
{
    public class Subscription
    {
        [Subscribe]
        public ExecutorStatus OnExecutorStatusChange(
            [Topic] string topicName,
            [EventMessage] ExecutorStatus executorStatus) => executorStatus;
        
        [Subscribe]
        public Flow OnFlowStart(
            [Topic] string topicName,
            [EventMessage] Flow flow) => flow;

    }
}