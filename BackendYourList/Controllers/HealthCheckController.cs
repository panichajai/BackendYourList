using BackendYourList.Data;
using BackendYourList.Models;
using BackendYourList.Models.Entities;
using BackendYourList.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BackendYourList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public HealthCheckController(ApplicationDbContext dbContext) 
        {
            this._dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetConnectDataBase()
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                result.data = _dbContext.customers.ToList();
            }
            catch (Exception ex) 
            {
                result.status = 500;
                result.success = false;
                result.message = ex.Message;
            }
            return Ok(result);
        }
    }
}
