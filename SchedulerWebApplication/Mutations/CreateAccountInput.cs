namespace SchedulerWebApplication.Mutations
{
    public class CreateAccountInput
    {
        public CreateAccountInput(string login, string password)
        {
            Login = login;
            Password = password;
        }
        
        public string Login { get; }
        public string Password { get; }
    }
}