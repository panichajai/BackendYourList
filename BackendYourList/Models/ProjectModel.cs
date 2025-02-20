using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendYourList.Models
{
    public class ProjectModel
    {
        public int id { get; set; }
        public required string name { get; set; }
        public required string details { get; set; }
        public string createBy { get; set; }
        public string updateBy { get; set; }
    }
    public class ProjectUpdateModel
    {
        public int id { get; set; }
        public required string name { get; set; }
        public required string details { get; set; }
        public string updateBy { get; set; }
    }
}
