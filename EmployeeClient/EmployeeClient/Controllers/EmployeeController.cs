using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EmployeeClient.Models;
using System.Web.Mvc;

namespace EmployeeClient.Controllers
{
    public class EmployeeController : Controller
    {

        string Baseurl = "http://localhost:55709//api/";
        // GET: Employee
        public ActionResult Index()
        {
            Baseurl = Baseurl + "People";
            List<EmployeeViewModel> empsList = new List<EmployeeViewModel>();

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Baseurl);
                request.ContentType = "application/json";
                request.KeepAlive = false;
                WebResponse response = request.GetResponse();
                string responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                empsList = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(responseStr);
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
           
            return View(empsList);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            Baseurl = Baseurl + "People/";
            Baseurl = Baseurl + id;
            EmployeeViewModel employeeviewModel = new EmployeeViewModel();
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Baseurl);
                request.ContentType = "application/json";
                request.KeepAlive = false;
                WebResponse response = request.GetResponse();
                string responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                employeeviewModel = JsonConvert.DeserializeObject<EmployeeViewModel>(responseStr);

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return View(employeeviewModel);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public  ActionResult Create(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url = Baseurl + "People";
                    var client = new HttpClient();
                    var values = new Dictionary<string, string>()
                    {
                        {"PersonId",employee.PersonId.ToString()},
                        {"FirstName" ,employee.Person.FirstName},
                        {"LastName",employee.Person.LastName},
                        {"BirthDate",employee.Person.BirthDate.ToString() },
                        {"EmployeeId",employee.EmployeeId.ToString() },
                        {"EmployeeNum", employee.EmployeeNum},
                        {"EmployeeDate",employee.EmployeeDate.ToString() },
                        //{"Terminated",person.Employee.Terminated.ToString() },
                    
                    };

                    var content = new FormUrlEncodedContent(values);
                    var response = client.PostAsync(url, content);
                    var result = response.Result.EnsureSuccessStatusCode();

                }

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int Id)
        {
            Baseurl = Baseurl + "People/";
            Baseurl = Baseurl + Id;
            EmployeeViewModel person = new EmployeeViewModel ();
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Baseurl);
                request.ContentType = "application/json";
                request.KeepAlive = false;
                WebResponse response = request.GetResponse();
                string responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                person = JsonConvert.DeserializeObject<EmployeeViewModel>(responseStr);

            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }

            return View(person);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(EmployeeViewModel empViewMod)
        {
            try
            {
                // TODO: Add update logic here

                Baseurl = Baseurl + "People/";
                Baseurl = Baseurl + empViewMod.EmployeeId;
                var client = new HttpClient();
                var values = new Dictionary<string, string>()
                    {
                        {"PersonId",empViewMod.PersonId.ToString()},
                        {"FirstName",empViewMod.FirstName},
                        {"LastName",empViewMod.LastName},
                        {"BirthDate",empViewMod.BirthDate.ToString() },
                        {"EmployeeId",empViewMod.EmployeeId.ToString() },
                        {"EmployeeNum",empViewMod.EmployeeNum},
                        {"EmployeeDate",empViewMod.EmployeeDate.ToString() },
                        {"Terminated",empViewMod.Terminated.ToString() }

                    };

                HttpContent content = new FormUrlEncodedContent(values);
                var request = new HttpRequestMessage(HttpMethod.Put, Baseurl)
                {
                    Content = content
                };
                await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            Baseurl = Baseurl + "People/";
            Baseurl = Baseurl + id;
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            Employee employee = new Employee();
            employee.Person = new Person();
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Baseurl);
                request.ContentType = "application/json";
                request.KeepAlive = false;
                WebResponse response = request.GetResponse();
                string responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                employeeViewModel = JsonConvert.DeserializeObject<EmployeeViewModel>(responseStr);

             
                employee.Person.PersonId = employeeViewModel.PersonId;
                employee.Person.FirstName = employeeViewModel.FirstName;
                employee.Person.LastName = employeeViewModel.LastName;
                employee.Person.BirthDate = employeeViewModel.BirthDate;
                employee.EmployeeId = employeeViewModel.EmployeeId;
                employee.EmployeeNum = employeeViewModel.EmployeeNum;
                employee.EmployeeDate = employeeViewModel.EmployeeDate;
                employee.PersonId = employeeViewModel.PersonId;

                if(employeeViewModel.Terminated != null)
                {
                    employee.Terminated = employeeViewModel.Terminated;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int? EmployeeId, EmployeeViewModel employee)
        {
            try
            {
                Baseurl = Baseurl + "People/";
                Baseurl = Baseurl + employee.EmployeeId;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(Baseurl);
                request.ContentType = "application/json";
                request.KeepAlive = false;
                request.Method = "Delete";
                WebResponse response = request.GetResponse();
                string responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return RedirectToAction("Index");
                // TODO: Add delete logic here
         
            }
            catch
            {
                return View();
            }
        }
    }
}
