using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectShopManagment.ViewModel
{
    public class EmployeeVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public Nullable<int> DesigID { get; set; }
        public Nullable<System.DateTime> Join_Date { get; set; }
        public string Picture { get; set; }
        [NotMapped]
        public HttpPostedFileBase Imgfile { get; set; }
    }

    public enum Gender { Male,Female}
}