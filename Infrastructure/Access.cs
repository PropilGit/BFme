using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Infrastructure
{
    public class Access
    {
        public Access(string server, string login, string password)
        {
            Server = server;
            Login = login;
            Password = password;
        }

        public string Server { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }
    }
}
