﻿namespace SchedulerWebApplication.Mutations
{
    public class AccountInput
    {
        public AccountInput(string login, string password)
        {
            Login = login;
            Password = password;
        }
        
        public string Login { get; }
        public string Password { get; }
    }
}