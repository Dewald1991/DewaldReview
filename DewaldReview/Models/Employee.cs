using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DewaldReview.Models
{
    public class Employee
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int EmploymentStatusID { get; set; }
        public virtual  EmploymentStatus EmploymentStatus { get; set; }


    }
}