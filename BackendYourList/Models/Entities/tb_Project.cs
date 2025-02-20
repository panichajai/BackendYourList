using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendYourList.Models.Entities
{
    public class tb_Project : CenterTable
    {
        [Key]
        public int id { get; set; }
        public required string name { get; set; }
        public required string details { get; set; }
        public string createBy { get; set; }
        public string updateBy { get; set; }
    }
}
