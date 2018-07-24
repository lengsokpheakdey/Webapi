using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {

        public IEnumerable<Employee> Get()
        {

            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var employee = entities.Employees.FirstOrDefault(e => e.ID == id);
                    return employee != null ? Request.CreateResponse(HttpStatusCode.OK, employee) : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id " + id + " not found");
                }
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception);
            }
        }

        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + employee.ID);

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var employee = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (employee != null)
                    {
                        entities.Employees.Remove(employee);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, employee);
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id = " + id + " not found to delete");
                }
            }
            catch (Exception exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, exception);
            }

        }

        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (entity != null)
                    {
                        entities.Entry(entity).State = EntityState.Modified;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id " + id + " not found to update");
                }
            }
            catch (Exception exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception);
            }

        }
    }
}
