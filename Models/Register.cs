using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace apiconsume.Models
{
    public class Register
    {

       
            public int Id { get; set; }
       [Required(ErrorMessage ="email id required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string Email { get; set; }
        [Required(ErrorMessage = "first name is required")]
        public string first_name { get; set; }
        [Required(ErrorMessage ="Last name is required")]
            public string last_name { get; set; }
        [Required(ErrorMessage ="file must be uploaded only .jpg and .png format only")]
            public string Avatar { get; set; }
         
        



    }
}