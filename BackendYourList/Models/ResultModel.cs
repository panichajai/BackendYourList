namespace BackendYourList.Models
{
    public class ResultModel
    {
        public int status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
