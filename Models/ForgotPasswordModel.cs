using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class ForgotPasswordModel
    {
        [Required, DataType(DataType.EmailAddress),
            RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$",
            ErrorMessage = "Invalid Email Address[Eg:sample@gmail.com]")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password),
                 RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$",
                 ErrorMessage = "Password must contain at least 1 uppercase letter," +
                 " 1 lowercase letter, and 1 number , can contain special characters" +
                 " should have atleast 8 characters")]
        public string NewPassword { get; set; }
    }
}