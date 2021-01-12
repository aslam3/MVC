using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectShopManagment.ViewModel
{
    public class ProfileVM
    {
        public string Address { get; set; }
        public int Age { get; set; }
        public string Fullname { get; set; }
        public string Phonenumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Pass { get; set; }
        public string SecStamp { get; set; }
        public string Picture { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImgFile { get; set; }
    }
}