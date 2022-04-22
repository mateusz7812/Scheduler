namespace SchedulerWebApplication.Mutations
{
    public class AccountOutput
    {
        public AccountOutput(int id, string login)
        {
            Id = id;
            Login = login;
        }
        
        public int Id { get; }
        public string Login { get; }
    }
}