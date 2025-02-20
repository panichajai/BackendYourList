using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendYourList.Models.Entities
{
    public class tb_Task : CenterTable
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }

        public int assigneeid { get; set; }
        [ForeignKey(nameof(assigneeid))]
        public virtual tb_Assignee Assignee { get; set; }

        public string status { get; set; }
        public string createBy { get; set; }
        public string updateBy { get; set; }
    }
}
