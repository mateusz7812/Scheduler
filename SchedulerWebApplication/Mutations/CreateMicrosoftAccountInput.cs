using System;

namespace SchedulerWebApplication.Mutations
{
    public class CreateMicrosoftAccountInput
    {
        public CreateMicrosoftAccountInput(string login, Guid microsoftAccountId)
        {
            Login = login;
            MicrosoftAccountId = microsoftAccountId;
        }
        
        public string Login { get; }
        public Guid MicrosoftAccountId { get; set; }
    }
}