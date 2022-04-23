namespace SchedulerWebApplication.Mutations
{
    public class ExecutorStatusInput
    {
        public int ExecutorId { get; set; }
        public ExecutorStatusCode StatusCode { get; set; }
        public int Date { get; set; }
    }
}