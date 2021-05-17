using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DewaldReview.DataBase;
using DewaldReview.Models;
using DewaldReview.Services.Clients;

namespace DewaldReview.Controllers
{
    public class EmployeesController : Controller
    {
        private ReviewContext db = new ReviewContext();

        // GET: Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.EmploymentStatus);
            return View(employees.ToList());
        }

        // GET: Employees
        public async Task <ActionResult> ExternalIndex()
        {
            var employees = await API.GetAllEmployees();
            return View(employees.ToList());
        }


        // GET: Employees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.EmploymentStatusID = new SelectList(db.EmploymentStatus, "EmploymentStatusID", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                string sExternalID = await API.CreateNewEmployee(employee.EmployeeID);
                if (!string.IsNullOrEmpty(sExternalID))
                {
                    employee.ExternalReference = sExternalID;
                    db.Entry(employee).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.EmploymentStatusID = new SelectList(db.EmploymentStatus, "EmploymentStatusID", "Name", employee.EmploymentStatusID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmploymentStatusID = new SelectList(db.EmploymentStatus, "EmploymentStatusID", "Name", employee.EmploymentStatusID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                await API.UpdateEmployeeStatus(employee.EmployeeID);
                
                return RedirectToAction("Index");
            }
            ViewBag.EmploymentStatusID = new SelectList(db.EmploymentStatus, "EmploymentStatusID", "Name", employee.EmploymentStatusID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Employee employee = db.Employees.Find(id);
            await API.DeleteExternalEmployee(employee.EmployeeID);

            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
