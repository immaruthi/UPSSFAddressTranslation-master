using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RMG.Models;
namespace RMG.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        //EmployeeController objEmployee = new EmployeeController();

        [HttpGet("[action]")]
        public IEnumerable<RMG.Models.Customer> GetAllCustomer()
        {
            CustomerContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.CustomerContext)) as CustomerContext;

            return context.GetAllCustomer();
        }

        [HttpPost("[action]")]
        //[Route("api/customer/create")]
        public int AddCustomer([FromBody]List<RMG.Models.Customer> customers)
        {
            CustomerContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.CustomerContext)) as CustomerContext;
            foreach (RMG.Models.Customer c in customers)
            {
                context.AddCustomer(c);
            }
            return 1;

        }

        [HttpPut("[action]")]
        // [Route("api/customer/Edit")]
        public int UpdateCustomer([FromBody]Customer customer)
        {
            CustomerContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.CustomerContext)) as CustomerContext;
            context.UpdateCustomer(customer);
            return 1;
        }


        [HttpGet("[action]")]
        // [Route("api/customer/Delete")]
        public int DeleteCustomer(string cust_id)
        {
            CustomerContext context = HttpContext.RequestServices.GetService(typeof(RMG.Models.CustomerContext)) as CustomerContext;
            context.DeleteCustomer(cust_id);
            return 1;
        }
    }
}