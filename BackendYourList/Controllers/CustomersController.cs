using System.Security.Cryptography;
using System.Text;
using BackendYourList.Models;
using BackendYourList.Models.Entities;
using BackendYourList.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendYourList.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _service;
        private readonly IConfiguration _configuration;

        public CustomersController(ICustomersService service, IConfiguration configuration)
        {
            this._service = service;
            this._configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var result = _service.GetCustomers();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetCustomerById(int id)
        {
            var result = _service.GetCustomerById(id);
            return Ok(result);
        }

        [HttpGet("GetByEmail")]
        public IActionResult GetCustomerByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Email is required."
                });
            }

            var result = _service.GetCustomerByEmail(email);
            if (!result.success)
            {
                return StatusCode(result.status, new
                {
                    success = result.success,
                    message = result.message
                });
            }

            return Ok(new
            {
                success = result.success,
                data = result.data
            });
        }


        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] CustomerRequestModel request)
        {
            var hashedPassword = HashPassword(request.password);

            var customer = new CustomerModel
            {
                fname = request.fname,
                lname = request.lname,
                email = request.email,
                password = hashedPassword, // ใช้รหัสผ่านที่เข้ารหัสแล้ว
                fullname = $"{request.fname} {request.lname}"
            };

            var result = _service.AddCustomer(customer);

            if (!result.success)
            {
                return StatusCode(result.status, result);
            }

            return Ok(result);
        }

        // ฟังก์ชันการเข้ารหัสรหัสผ่าน
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult AddCustomer(CustomerModel customer)
        //{
        //    //customer.password = BCrypt.Net.BCrypt.HashPassword(customer.password);
        //    var result = _service.AddCustomer(customer);
        //    return Ok(result);
        //}
        [HttpGet("{id}/passwordHash")]
        public IActionResult GetPasswordHash(int id)
        {
            // ดึงข้อมูลลูกค้าจาก Service
            var result = _service.GetCustomerById(id);

            if (!result.success || result.data == null)
            {
                return NotFound(new { success = false, message = "Customer not found." });
            }

            // สมมติว่า password เก็บใน result.data
            var customer = result.data as CustomerModel;
            if (customer == null || string.IsNullOrEmpty(customer.password))
            {
                return BadRequest(new { success = false, message = "Password hash not found." });
            }

            return Ok(new { passwordHash = customer.password });
        }

        [HttpPost("{id}/verifyPassword")]
        public IActionResult VerifyPassword(int id, [FromBody] VerifyPasswordModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.currentpassword))
            {
                return BadRequest(new { success = false, message = "Invalid input data." });
            }

            // ดึงข้อมูลลูกค้าเพื่อตรวจสอบรหัสผ่าน
            var result = _service.GetCustomerById(id);
            if (!result.success || result.data == null)
            {
                return NotFound(new { success = false, message = "Customer not found." });
            }

            var customer = result.data as CustomerModel;
            if (customer == null || string.IsNullOrEmpty(customer.password))
            {
                return BadRequest(new { success = false, message = "Invalid customer data." });
            }

            // ตรวจสอบรหัสผ่านที่ส่งมาว่าตรงกับที่เก็บไว้หรือไม่
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(model.currentpassword, customer.password);
            if (!isPasswordValid)
            {
                return Unauthorized(new { success = false, message = "Invalid password." });
            }

            return Ok(new { success = true, message = "Password verified successfully." });
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateCustomer(int id, [FromBody] CustomerUpdateModel model)
        {
            if (model == null)
            {
                return BadRequest(new { success = false, message = "Invalid input data." });
            }

            // อัปเดตข้อมูลลูกค้า
            var result = _service.UpdateCustomer(id, model);
            if (!result.success)
            {
                return StatusCode(result.status, result);
            }

            return Ok(result);
        }

        //[HttpPut("{id:int}")]
        //public IActionResult UpdateCustomer(int id, [FromBody] CustomerUpdateModel model)
        //{
        //    if (model == null)
        //    {
        //        return BadRequest(new { success = false, message = "Invalid input data." });
        //    }

        //    var result = _service.UpdateCustomer(id, model);
        //    if (!result.success)
        //    {
        //        return StatusCode(result.status, result);
        //    }

        //    return Ok(result);
        //}

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteCustomer(int id)
        {
            var result = _service.DeleteCustomer(id);
            return Ok(result);
        }

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] LoginRequest request)
        //{
        //    if (request == null || string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
        //    {
        //        return BadRequest(new
        //        {
        //            status = 400,
        //            success = false,
        //            message = "Email and Password are required."
        //        });
        //    }

        //    var result = _service.Login(request.email, request.password);

        //    if (!result.success)
        //    {
        //        return Unauthorized(new
        //        {
        //            status = 401,
        //            success = false,
        //            message = "Invalid email or password."
        //        });
        //    }

        //    return Ok(result);
        //}
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
            {
                return BadRequest(new
                {
                    status = 400,
                    success = false,
                    message = "Email and Password are required."
                });
            }

            // ดึงข้อมูลลูกค้าจากอีเมล
            var result = _service.GetCustomerByEmail(request.email);
            if (!result.success || result.data == null)
            {
                return Unauthorized(new
                {
                    status = 401,
                    success = false,
                    message = "Invalid email or password."
                });
            }

            // แปลง result.data ให้เป็น CustomerModel
            var customer = result.data as CustomerModel;
            if (customer == null)
            {
                return Unauthorized(new
                {
                    status = 401,
                    success = false,
                    message = "Invalid email or password."
                });
            }

            // ตรวจสอบรหัสผ่าน
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.password, customer.password);
            if (!isPasswordValid)
            {
                return Unauthorized(new
                {
                    status = 401,
                    success = false,
                    message = "Invalid email or password."
                });
            }

            // ตอบกลับเมื่อล็อกอินสำเร็จ
            return Ok(new
            {
                status = 200,
                success = true,
                message = "Login successful."
            });
        }



        //[HttpPost("login")]
        //public IActionResult Login(string email, string password)
        //{
        //    //// อ่านค่าจาก appsettings.json
        //    //var jwtSettings = _configuration.GetSection("Jwt");
        //    //var jwtKey = jwtSettings["Key"];
        //    //var issuer = jwtSettings["Issuer"];
        //    //var audience = jwtSettings["Audience"];

        //    //var result = _service.Login(email, password, jwtKey, issuer, audience);
        //    var result = _service.Login(email, password);

        //    if (!result.success)
        //    {
        //        return Unauthorized(result);
        //    }

        //    return Ok(result);
        //}

    }
}
