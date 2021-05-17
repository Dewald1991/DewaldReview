using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DewaldReview.Services.APIModels
{
    public class APIModels
    {
        public class APIEmployee
        {
            public string EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public int EmploymentStatusID { get; set; }
            public virtual APIEmploymentStatus EmploymentStatus { get; set; }


        }
        public class APIEmploymentStatus
        {
            public int EmploymentStatusID { get; set; }
            public string Name { get; set; }
        }



    }
}