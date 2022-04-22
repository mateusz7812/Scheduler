namespace SchedulerWebApplication.Mutations
{
    public class TaskInput
    {
        public string InputType { get; set; }
        public string OutputType { get; set; }
        public string Name { get; set; }
        
        public string Command { get; set; }
    }
}