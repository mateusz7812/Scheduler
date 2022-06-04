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
        public FlowTaskStatus OnFlowRunTasksStatusChange(
            [Topic] string topicName,
            [EventMessage] FlowTaskStatus flowTaskStatus) => flowTaskStatus;

        [Subscribe]
        public FlowRun OnFlowStart(
            [Topic] string topicName,
            [EventMessage] FlowRun flowRun) => flowRun;

    }
}