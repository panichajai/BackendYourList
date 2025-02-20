using System.ComponentModel.DataAnnotations;

namespace BackendYourList.Models.Entities
{
    public class tb_Customer : CenterTable
    {
        [Key]
        public int id { get; set; }
        public string fname { get; set; }

        public string lname { get; set; }

        //[EmailAddress] 
        public string email { get; set; }

        public string password { get; set; }
    }
}
