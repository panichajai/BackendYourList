using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendYourList.Models.Entities
{
    public class tb_Assignee : CenterTable
    {
        [Key]
        public int id { get; set; }
        public int projectid { get; set; }
        [ForeignKey(nameof(projectid))]
        public virtual tb_Project project { get; set; }
        public int customerid { get; set; }
        [ForeignKey(nameof(customerid))]
        public virtual tb_Customer customer { get; set; }
    }

}
