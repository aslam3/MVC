using ProjectShopManagment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectShopManagment.ViewModel
{
    public class ManagerVm
    {
        public int ID { get; set; }
        public string ManagerName { get; set; }
        public string Email { get; set; }
        public Nullable<int> M_Stall { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public HttpPostedFileBase Imgfile { get; set; }

        public virtual Stall Stall { get; set; }
    }
}