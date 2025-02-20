using BackendYourList.Models;
using BackendYourList.Models.Entities;

namespace BackendYourList.Services.Interface
{
    public interface ICustomersService
    {
        ResultModel GetCustomers();
        ResultModel GetCustomerById(int id);
        ResultModel GetCustomerByEmail(string email);
        ResultModel AddCustomer(CustomerModel model);
        ResultModel UpdateCustomer(int id, CustomerUpdateModel model);
        ResultModel DeleteCustomer(int id);
        ResultModel Login(string email, string password);
        //ResultModel Login(string email, string password, string jwtKey, string issuer, string audience);
    }
}
