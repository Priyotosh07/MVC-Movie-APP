using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class UserEailOptions
    {
        public List<string> ToEmails { get; set; }
        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
