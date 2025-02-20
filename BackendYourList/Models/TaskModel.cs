using System.ComponentModel.DataAnnotations;
using BackendYourList.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendYourList.Models
{
    public class TaskModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public int assigneeid { get; set; }
        public string status { get; set; }
        public string createBy { get; set; }
        public string updateBy { get; set; }
    }

    public class TaskrUpdateModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string status { get; set; }
        public int assigneeid { get; set; }
        public string updateBy { get; set; }
    }
}
