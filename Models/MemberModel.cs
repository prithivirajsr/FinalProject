using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace FinalProject.Models
{
    public class MemberModel
    {
        [Key]
        public int MemberId { get; set; }

        [Required, DataType(DataType.Text),
            RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name field accepts only alphabets"),
            MaxLength(25, ErrorMessage = "Name must contain below 25 characters"),
            MinLength(3, ErrorMessage = "Name must contain atleast 3 characters"),
            DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required]
        public string Profession { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, DataType(DataType.PhoneNumber),
           RegularExpression(@"\d{10}", ErrorMessage = "Mobile No. must contain only 10 digits"),
           DisplayName("Mobile No.")]
        public string MobileNo  { get; set; }

        [Required]
        public string City { get; set; }

        [Required, DataType(DataType.PostalCode),
           RegularExpression(@"\d{6}", ErrorMessage = "Pincode must contain only 6 digits")
           ]
        public int Pincode { get; set; }

        [Required, DataType(DataType.EmailAddress),
            RegularExpression(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$",
            ErrorMessage = "Invalid Email Address[Eg:sample@gmail.com]")]
        public string Email { get; set; }
    }


}