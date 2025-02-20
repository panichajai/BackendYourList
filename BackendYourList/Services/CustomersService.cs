using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendYourList.Data;
using BackendYourList.Models;
using BackendYourList.Models.Entities;
using BackendYourList.Services.Interface;
using Microsoft.IdentityModel.Tokens;

namespace BackendYourList.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomersService(ApplicationDbContext dbContext) 
        {
            this._dbContext = dbContext;
        }
        public ResultModel AddCustomer(CustomerModel model)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };

            try
            {
                var existingCustomer = _dbContext.customers.FirstOrDefault(e => e.email.Trim() == model.email.Trim());
                if (existingCustomer != null)
                {
                    result.success = false;
                    result.message = "A customer with this email already exists.";
                    result.status = 409;
                    return result;
                }

                var customerEntity = new tb_Customer
                {
                    fname = model.fname,
                    lname = model.lname,
                    email = model.email,
                    password = model.password,
                    createBy = "system",
                    createDate = DateTime.UtcNow,
                    updateBy = "admin",
                    updateDate = DateTime.UtcNow, 
                    isDelete = false
                };

                _dbContext.customers.Add(customerEntity);
                _dbContext.SaveChanges();

                result.data = customerEntity;
            }
            catch (Exception ex)
            {
                result.status = 500;
                result.success = false;
                result.message = ex.Message;
            }

            return result;
        }

        //public ResultModel AddCustomer(CustomerModel model) 
        //{
        //    var result = new ResultModel
        //    {
        //        status = 200,
        //        success = true,
        //    };
        //    try
        //    {
        //        var existingCustomer = _dbContext.customers.FirstOrDefault(e => e.email.Trim() == model.email.Trim());
        //        if (existingCustomer != null)
        //        {
        //            result.success = false;
        //            result.message = "A customer with this email already exists." + existingCustomer.email;
        //            result.status = 501;
        //            return result;
        //        }
        //        if (string.IsNullOrWhiteSpace(model.password))
        //        {
        //            result.success = false;
        //            result.message = "Password is required.";
        //            result.status = 400; // Bad Request
        //            return result;
        //        }

        //        //// Hash รหัสผ่านก่อนบันทึก
        //        //var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(model.password);
        //        var customerEntity = new tb_Customer()
        //        {
        //            fname = model.fname,
        //            lname = model.lname,
        //            email = model.email,
        //            password = model.password,
        //            createBy = "admin",
        //            createDate = DateTime.UtcNow, 
        //            updateBy = "admin",
        //            updateDate = DateTime.UtcNow, 
        //            isDelete = false,

        //        };

        //        _dbContext.customers.Add(customerEntity);
        //        _dbContext.SaveChanges();
        //        result.data = customerEntity;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultModel
        //        {
        //            status = 500,
        //            success = false,
        //            message = $"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}",
        //            data = ex.StackTrace  // เพิ่มข้อมูล StackTrace สำหรับ Debug
        //        };
        //    }
        //    return result;
        //}

        public ResultModel DeleteCustomer(int id)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var customer = _dbContext.customers.Find(id);

                if (customer is null)
                {
                    result.success = false;
                    result.message = "Not Have Customer ById : " + id;
                    result.status = 501;
                    return result;
                }
                customer.isDelete = true;
                _dbContext.customers.Update(customer);
                //_dbContext.customers.Remove(customer);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    status = 500,
                    success = false,
                    message = ex.Message
                };
            }
            return result;
        }

        //public ResultModel GetCustomerById(int id)
        //{
        //    var result = new ResultModel
        //    {
        //        status = 200,
        //        success = true,
        //    };
        //    try
        //    {
        //        var customer = _dbContext.customers.Find(id);
        //        if (customer is null)
        //        {
        //            result.success = false;
        //            result.message = "Not Have Customer ById : " + id;
        //            result.status = 501;
        //            return result;
        //        }
        //        result.data = customer;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultModel
        //        {
        //            status = 500,
        //            success = false,
        //            message = ex.Message
        //        };
        //    }
        //    return result;
        //}

        //public ResultModel GetCustomerByEmail(string email)
        //{
        //    var result = new ResultModel
        //    {
        //        status = 200,
        //        success = true,
        //    };
        //    try
        //    {
        //        var customer = _dbContext.customers.Where(w=>w.email == email).FirstOrDefault();
        //        if (customer is null)
        //        {
        //            result.success = false;
        //            result.message = "Not Have Customer ByEmail : " + email;
        //            result.status = 501;
        //            return result;
        //        }
        //        result.data = customer;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultModel
        //        {
        //            status = 500,
        //            success = false,
        //            message = ex.Message
        //        };
        //    }
        //    return result;
        //}
        public ResultModel GetCustomerById(int id)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };

            try
            {
                // ดึงข้อมูลจากฐานข้อมูล
                var customerEntity = _dbContext.customers.Find(id);

                // ตรวจสอบว่าพบข้อมูลหรือไม่
                if (customerEntity == null)
                {
                    result.success = false;
                    result.message = $"Customer with ID {id} not found.";
                    result.status = 404; // เปลี่ยนจาก 501 เป็น 404
                    return result;
                }

                // แปลงข้อมูลจาก Entity เป็น Model
                result.data = new CustomerModel
                {
                    id = customerEntity.id,
                    fname = customerEntity.fname,
                    lname = customerEntity.lname,
                    email = customerEntity.email,
                    password = customerEntity.password, // ตรวจสอบว่าค่านี้ไม่เป็น null
                };
            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาด
                result.status = 500;
                result.success = false;
                result.message = $"An error occurred: {ex.Message}";
            }

            return result;
        }

        //public ResultModel GetCustomerByEmail(string email)
        //{
        //    var result = new ResultModel
        //    {
        //        status = 200,
        //        success = true,
        //    };
        //    try
        //    {
        //        var customer = _dbContext.customers.FirstOrDefault(w => w.email == email && !w.isDelete);
        //        if (customer == null)
        //        {
        //            result.success = false;
        //            result.message = $"No customer found with email: {email}";
        //            result.status = 404;
        //            return result;
        //        }
        //        result.data = new CustomerModel
        //        {
        //            id = customer.id,
        //            fname = customer.fname,
        //            lname = customer.lname,
        //            email = customer.email,
        //            fullname = $"{customer.fname} {customer.lname}",
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        result.status = 500;
        //        result.success = false;
        //        result.message = $"Error fetching customer: {ex.Message}";
        //    }
        //    return result;
        //}
        public ResultModel GetCustomerByEmail(string email)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var customer = _dbContext.customers.FirstOrDefault(w => w.email == email && !w.isDelete);
                if (customer == null)
                {
                    result.success = false;
                    result.message = $"No customer found with email: {email}";
                    result.status = 404;
                    return result;
                }
                result.data = new CustomerModel
                {
                    id = customer.id,
                    fname = customer.fname,
                    lname = customer.lname,
                    email = customer.email,
                    fullname = $"{customer.fname} {customer.lname}",
                    password = customer.password
                };
            }
            catch (Exception ex)
            {
                result.status = 500;
                result.success = false;
                result.message = $"Error fetching customer: {ex.Message}";
            }
            return result;
        }

        public ResultModel GetCustomers()
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var customers = _dbContext.customers
                    .Where(s => !s.isDelete)
                    .Select(s => new CustomerModel
                {
                    id = s.id,
                    fname = s.fname,
                    lname = s.lname,
                    fullname = s.fname+" "+s.lname,
                    email = s.email,
                    password = s.password,
                }).ToList();
                result.data = customers;
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    status = 500,
                    success = false,
                    message = ex.Message
                };
            }
            return result;
        }

        //public ResultModel UpdateCustomer(int id, CustomerUpdateModel model)
        //{
        //    var result = new ResultModel
        //    {
        //        status = 200,
        //        success = true,
        //    };
        //    try
        //    {
        //        var Customer = _dbContext.customers.Find(id);

        //        if (Customer is null)
        //        {
        //            result.success = false;
        //            result.message = "Not Have Customer ById : " + id;
        //            result.status = 501;
        //            return result;
        //        }
        //        Customer.fname = model.fname;
        //        Customer.lname = model.lname;
        //        Customer.password = model.password;
        //        //if (!string.IsNullOrEmpty(model.password))
        //        //{
        //        //    // Hash รหัสผ่านใหม่และบันทึก
        //        //    Customer.password = BCrypt.Net.BCrypt.HashPassword(model.password);
        //        //}

        //        _dbContext.SaveChanges();
        //        result.data = model;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultModel
        //        {
        //            status = 500,
        //            success = false,
        //            message = $"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}",
        //        };
        //    }
        //    return result;
        //}
        
        public ResultModel UpdateCustomer(int id, CustomerUpdateModel model)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };

            try
            {
                // ค้นหาข้อมูลลูกค้าในฐานข้อมูล
                var customer = _dbContext.customers.Find(id);

                if (customer is null)
                {
                    result.success = false;
                    result.message = "Customer not found with ID: " + id;
                    result.status = 404;
                    return result;
                }

                // ตรวจสอบความถูกต้องของรหัสผ่านเดิม (ถ้ามีการส่งมา)
                if (!string.IsNullOrEmpty(model.currentpassword))
                {
                    var isPasswordValid = BCrypt.Net.BCrypt.Verify(model.currentpassword, customer.password);
                    if (!isPasswordValid)
                    {
                        return new ResultModel
                        {
                            success = false,
                            message = "Current password is incorrect.",
                            status = 400
                        };
                    }
                }

                // อัปเดตข้อมูลลูกค้า
                customer.fname = model.fname;
                customer.lname = model.lname;

                // ตรวจสอบและตั้งค่ารหัสผ่านใหม่ (ถ้ามีการส่งมา)
                if (!string.IsNullOrEmpty(model.password))
                {
                    customer.password = BCrypt.Net.BCrypt.HashPassword(model.password);
                }

                // บันทึกการเปลี่ยนแปลงในฐานข้อมูล
                _dbContext.SaveChanges();

                result.data = new
                {
                    customer.fname,
                    customer.lname
                };
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    status = 500,
                    success = false,
                    message = $"Error: {ex.Message}, Inner Exception: {ex.InnerException?.Message}",
                };
            }

            return result;
        }

        //public ResultModel Login(string email, string password, string jwtKey, string issuer, string audience)
        public ResultModel Login(string email, string password)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };

            try
            {
                // ตรวจสอบว่ามีผู้ใช้งานนี้หรือไม่
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    result.success = false;
                    result.message = "Email or password cannot be empty.";
                    result.status = 400;
                    return result;
                }

                var customer = _dbContext.customers.FirstOrDefault(e => e.email == email);
                if (customer == null)
                {
                    result.success = false;
                    result.message = "Invalid email or password.";
                    result.status = 401;
                    return result;
                }

                //// ตรวจสอบรหัสผ่านโดย Verify กับ Password ที่ถูก Hash
                //if (string.IsNullOrEmpty(customer.password))
                //{
                //    result.success = false;
                //    result.message = "Password not found in database.";
                //    result.status = 500; 
                //    return result;
                //}

                //bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, customer.password);
                //if (!isPasswordValid)
                //{
                //    result.success = false;
                //    result.message = "Invalid email or password.";
                //    result.status = 401;
                //    return result;
                //}

                //// สร้าง JWT Token
                //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                //var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                //var claims = new[]
                //{
                //    new Claim("CustomerId", customer.id.ToString()), // Custom Claim สำหรับ CustomerId
                //    new Claim("Email", customer.email),
                //};

                //var token = new JwtSecurityToken(
                //    issuer: issuer,
                //    audience: audience,
                //    claims: claims,
                //    expires: DateTime.UtcNow.AddHours(1), // Token หมดอายุใน 1 ชม.
                //    signingCredentials: credentials
                //);

                //var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                //result.message = "Login successful!";
                //result.data = new { Token = tokenString }; // ส่ง Token กลับ
            }
            catch (SecurityTokenException ex)
            {
                result.status = 500;
                result.success = false;
                result.message = $"Token generation error: {ex.Message}";
            }
            catch (Exception ex)
            {
                result.status = 500;
                result.success = false;
                result.message = $"Unexpected error: {ex.Message}, StackTrace: {ex.StackTrace}";
            }

            return result;
        }

    }
}
