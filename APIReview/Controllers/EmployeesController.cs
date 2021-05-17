using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using APIReview.DataBase;
using APIReview.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace APIReview.Controllers
{
    public class EmployeesController : ApiController
    {
        public  readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private ReviewContext db = new ReviewContext();

        // GET: api/Employees
        public HttpResponseMessage Get()
        {
            Logger.Debug("Get Requested");
            List<Employee> Employees = db.Employees.ToList();


            string ReturnedEmployees = JsonConvert.SerializeObject(Employees);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);

            Logger.Debug("Employees sending back: "+ ReturnedEmployees);
            response.Content = new StringContent(ReturnedEmployees, Encoding.UTF8, "application/json");
            return response;

        }

        // GET: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult GetEmployee(string id)
        {
            Logger.Debug("Employee records requested for: "+ id);
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(string id, Employee employee)
        {
            Logger.Debug("Request to edit employee: "+ id);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                Logger.Debug("Changed to Status: " + employee.EmploymentStatusID);
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.Debug("Failed to update: "+ e.Message);
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employees
        [ResponseType(typeof(Employee))]
        public HttpResponseMessage PostEmployee(Employee employee)
        {
            Logger.Debug("New Employee: "+ employee);
            if (!ModelState.IsValid)
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest); 
            }

            employee.EmployeeID = Guid.NewGuid().ToString();

           
            db.Employees.Add(employee);

            try
            {
                db.SaveChanges();
                Logger.Debug("Employee added with ID: "+employee.EmployeeID);
            }
            catch (DbUpdateException)
            {
                if (EmployeeExists(employee.EmployeeID))
                {
                    return this.Request.CreateResponse(HttpStatusCode.Conflict);
                }
                else
                {
                    throw;
                }
            }
            


            

            string ReturnedEmployees = JsonConvert.SerializeObject(employee);
            var response = this.Request.CreateResponse(HttpStatusCode.OK);


            response.Content = new StringContent(ReturnedEmployees, Encoding.UTF8, "application/json");
            return response;
        }

        // DELETE: api/Employees/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(string id)
        {
            Logger.Debug("Request to delete employee with ID: "+ id);
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();
            Logger.Debug("Employee Deleted");
            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(string id)
        {
            return db.Employees.Count(e => e.EmployeeID == id) > 0;
        }
    }
}