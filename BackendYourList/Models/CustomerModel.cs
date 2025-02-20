namespace BackendYourList.Models
{
    public class CustomerModel
    {
        public int id { get; set; }
        public  string fname { get; set; }
        public  string lname { get; set; }
        public  string fullname { get; set; }
        public  string email { get; set; }
        public  string password { get; set; }
    }
    public class CustomerUpdateModel
    {
        public string fname { get; set; }
        public string lname { get; set; }
        public string currentpassword { get; set; } 
        public string password { get; set; }
    }
    public class VerifyPasswordModel
    {
        public string currentpassword { get; set; }
    }

    public class CustomerRequestModel
    {
        public string fname { get; set; }
        public string lname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }

    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }
    }

}
