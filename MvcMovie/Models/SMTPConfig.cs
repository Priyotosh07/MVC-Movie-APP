using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class SMTPConfig
    {

        public String SenderAdress { get; set; }

        public String SenderDisplayName { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool IsBodyHTML { get; set; }
        

    }
}
