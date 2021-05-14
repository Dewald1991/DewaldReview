using DewaldReview.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;


namespace DewaldReview.DataBase
{
    public class Seed
    {
        
        public void SeedData(DewaldReview.DataBase.ReviewContext context)
        {
            try
            {
                context.EmploymentStatus.AddOrUpdate
                    (
                         p => p.Name,
                         new EmploymentStatus() { Name = "Employeed" , EmploymentStatusID =1},
                         new EmploymentStatus() { Name = "ToBeHired", EmploymentStatusID = 2 },
                         new EmploymentStatus() { Name = "Rejected", EmploymentStatusID = 3 },
                         new EmploymentStatus() { Name = "Fired", EmploymentStatusID = 4 }


                       );
                context.SaveChanges();
                context.Employees.AddOrUpdate
                    (p => p.EmployeeName,
                    new Employee()
                    {
                        EmployeeName = "Phillip",
                        EmploymentStatusID = new Random().Next(4),
                        EmployeeID = Guid.NewGuid().ToString()
                    },
                    new Employee()
                    {
                        EmployeeName = "Dewald",
                        EmploymentStatusID = new Random().Next(4),
                        EmployeeID = Guid.NewGuid().ToString()
                    }
                    );                 
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder oSb = new StringBuilder();

                foreach (var eve in e.EntityValidationErrors)
                {

                    oSb.AppendFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //oSb.AppendFormat("- Property: \"{0}\", Error: \"{1}\"",
                        //    ve.PropertyName, ve.ErrorMessage);
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw new Exception(oSb.ToString(), e);
            }
            catch (Exception ex) { throw ex; }
        }
    }
}