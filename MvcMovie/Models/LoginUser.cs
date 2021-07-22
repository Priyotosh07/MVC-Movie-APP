using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class LoginUser
    {
        [Key]
        public int ID { get; set; }
        [Required, EmailAddress]
        [Display(Name ="Email Adress")]
        public String Email { get; set;}

        [Required, DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }

       // public string ReturnUrl { get; set; }

       // public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
