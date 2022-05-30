namespace SchedulerWebApplication.Mutations
{
    public class CreateLocalAccountInput
    {
        public CreateLocalAccountInput(string login, string password)
        {
            Login = login;
            Password = password;
        }
        
        public string Login { get; }
        public string Password { get; }
    }
}