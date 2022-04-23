namespace SchedulerWebApplication.Mutations
{
    public class CreateExecutorInput
    {
        public CreateExecutorInput(int accountId, string name, string description)
        {
            AccountId = accountId;
            Name = name;
            Description = description;
        }
        
        public int AccountId { get; }
        public string Name { get; }
        public string Description { get; }
    }
}