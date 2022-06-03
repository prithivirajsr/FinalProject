using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;


namespace FinalProject.Models
{
    public class EventModel
    {
        [Key]
        [Required]
        public int EventId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public string MemberName { get; set; }

        [Required, DataType(DataType.Text),
            RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name field accepts only alphabets"),
            MaxLength(25, ErrorMessage = "Name must contain below 25 characters"),
            MinLength(3, ErrorMessage = "Name must contain atleast 3 characters")]
        public string Venue { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string ECO { get; set; }

        [Required]
        public string GDC { get; set; }

        [Required]
        public string NSS { get; set; }

        public List<SelectListItem> SelectMember { get; set; }
    }
}
