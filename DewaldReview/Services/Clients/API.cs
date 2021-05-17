using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DewaldReview.DataBase;
using DewaldReview.Models;
using Newtonsoft.Json;
using NLog;

namespace DewaldReview.Services.Clients
{
    public class API
    {
        public static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static async Task<List<Employee>>  GetAllEmployees()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44307/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string Result = "";
                List < Employee > Employees = new List<Employee>();

                HttpResponseMessage response = await client.GetAsync("api/Employees");


                if (response.IsSuccessStatusCode)
                {
                    string sReturnedObject = response.Content.ReadAsStringAsync().Result;
                    string CleanedObject = Newtonsoft.Json.JsonConvert.DeserializeObject(sReturnedObject).ToString();
                    var ReturnedEmployees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<APIModels.APIModels.APIEmployee>>(sReturnedObject);
                    Logger.Debug("Returned "+CleanedObject);

                    foreach (var employee in ReturnedEmployees)
                    {

                        Employee Emp = CreateAPIEmployee(employee);
                        Employees.Add(Emp);

                    }
                    

                }
                else
                {
                    Logger.Debug(response.StatusCode.ToString());
                }
                return Employees;
            }        
        }
        public static Employee CreateAPIEmployee(APIModels.APIModels.APIEmployee APIEmployee)
        {

            var EmploymentStatus = new EmploymentStatus()
            {

                EmploymentStatusID = APIEmployee.EmploymentStatus.EmploymentStatusID,
                Name = APIEmployee.EmploymentStatus.Name

            };

            return new Employee()
            {
                EmployeeID = APIEmployee.EmployeeID,
                EmployeeName = APIEmployee.EmployeeName,
                EmploymentStatus = EmploymentStatus,
                EmploymentStatusID = APIEmployee.EmploymentStatusID

            };



        }


        public static APIModels.APIModels.APIEmployee CreateAPIEmployee(Employee employee)
        {

            var APIEmploymentStatus = new APIModels.APIModels.APIEmploymentStatus() {

                EmploymentStatusID = employee.EmploymentStatus.EmploymentStatusID,
                Name = employee.EmploymentStatus.Name

            };

            return new APIModels.APIModels.APIEmployee()
            {
                EmployeeID = employee.ExternalReference,
                EmployeeName = employee.EmployeeName,
                EmploymentStatus = APIEmploymentStatus,
                EmploymentStatusID = employee.EmploymentStatusID

            };



        }

        public static async Task<List<Employee>> UpdateEmployeeStatus(string id)
        {
            ReviewContext db = new ReviewContext();
            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri("https://localhost:44307/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string Result = "";
                List<Employee> Employee = new List<Employee>();
                Employee employee = db.Employees.Find(id);

                var APIEmployee = CreateAPIEmployee(employee);

                string URI = "api/Employees/" + employee.ExternalReference;
                
                string sAPIEmployee = JsonConvert.SerializeObject(APIEmployee);

                Logger.Debug("Employee to edit: "+sAPIEmployee);
                var content = new StringContent(sAPIEmployee, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(URI, content);


                if (response.IsSuccessStatusCode)
                {
                    Employee = await response.Content.ReadAsAsync<List<Employee>>();
                    Logger.Debug("Responce from API: " + JsonConvert.SerializeObject(Employee));
                }
                else
                {
                    Logger.Debug("Response from API " + response.ReasonPhrase);
                }
                return Employee;
            }
        }
        public static async Task<string> DeleteExternalEmployee(string id)
        {
            ReviewContext db = new ReviewContext();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://localhost:44307/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string Result = "";
                string ExternalID = "";
                Employee employee = db.Employees.Find(id);

                var APIEmployee = CreateAPIEmployee(employee);

                string URI = "api/Employees/";

                string sAPIEmployee = JsonConvert.SerializeObject(APIEmployee);
                Logger.Debug("Employee to Delete: "+ sAPIEmployee);

                var content = new StringContent(sAPIEmployee, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(URI, content);


                if (response.IsSuccessStatusCode)
                {
                    string sReturnedObject = response.Content.ReadAsStringAsync().Result;
                    var ReturnedEmployee = Newtonsoft.Json.JsonConvert.DeserializeObject<APIModels.APIModels.APIEmployee>(sReturnedObject);



                    ExternalID = ReturnedEmployee.EmployeeID;
                    Logger.Debug("Employee Deleted: " + ReturnedEmployee);
                }
                else
                {
                    Logger.Debug("Response from API: "+ response.ReasonPhrase);
                }
                return ExternalID;
            }
        }
        public static async Task<string> CreateNewEmployee(string id)
        {
            ReviewContext db = new ReviewContext();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://localhost:44307/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string Result = "";
               string ExternalID = "";
                Employee employee = db.Employees.Find(id);

                var APIEmployee = CreateAPIEmployee(employee);

                string URI = "api/Employees/";

                
                HttpResponseMessage response = await client.DeleteAsync(URI);
                

                if (response.IsSuccessStatusCode)
                {
                    string sReturnedObject = response.Content.ReadAsStringAsync().Result;
                    var ReturnedEmployee = Newtonsoft.Json.JsonConvert.DeserializeObject<APIModels.APIModels.APIEmployee>(sReturnedObject); 



                    ExternalID = ReturnedEmployee.EmployeeID;

                }
                return ExternalID;
            }
        }


        //static async Task RunAsync()
        //{


        //    using (var client = new HttpClient())
        //    {



        //        HttpResponseMessage response = await client.GetAsync("api/products/1");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Product product = await response.Content.ReadAsAsync<Product>();
        //            Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
        //        }

        //        // HTTP POST
        //        var gizmo = new Product() { Name = "Gizmo", Price = 100, Category = "Widget" };
        //        response = await client.PostAsJsonAsync("api/products", gizmo);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Uri gizmoUrl = response.Headers.Location;

        //            // HTTP PUT
        //            gizmo.Price = 80;   // Update price
        //            response = await client.PutAsJsonAsync(gizmoUrl, gizmo);

        //            // HTTP DELETE
        //            response = await client.DeleteAsync(gizmoUrl);
        //        }
        //    }
        //}
        //GET //Get ALL
        //    PUT //Edit
        //    POST //Create
        //    DELETE //Delete

    }
}