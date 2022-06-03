using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FinalProject.Models
{
    public class UserModel
    {
        [Required, DataType(DataType.Text),
            RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name field accepts only alphabets"),
            MaxLength(25,ErrorMessage = "Name must contain below 25 characters"),
            MinLength(3,ErrorMessage = "Name must contain atleast 3 characters"),
            ]
        public string Name { get; set; }


        [Required]
        public DateTime DateOfBirth { get; set; }


        [Required,DataType(DataType.PhoneNumber),
            RegularExpression(@"\d{10}",ErrorMessage = "Mobile No. must contain only 10 digits"),
            DisplayName("Mobile No.")]
        public string MobileNo { get; set; }


        [Required, DataType(DataType.EmailAddress),
            RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$",
            ErrorMessage = "Invalid Email Address[Eg:sample@gmail.com]")]
        public string Email { get; set; }


        [Required, DataType(DataType.Password),
            RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$",
            ErrorMessage = "Password must contain at least 1 uppercase letter," +
            " 1 lowercase letter, and 1 number , can contain special characters" +
            " should have atleast 8 characters")]
        public string Password { get; set; }


        [Required, DataType(DataType.Password),
            Compare("Password",ErrorMessage = "Password & Confirm Password must be same"),
            DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}