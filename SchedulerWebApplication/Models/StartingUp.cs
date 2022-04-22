namespace SchedulerWebApplication.Models
{
    public class StartingUp
    {
        public int SuccessorId { get; set; }
        public int PredecessorId { get; set; }
        
        public FlowTask Successor { get; set; }
        public FlowTask Predecessor { get; set; }
    }
}