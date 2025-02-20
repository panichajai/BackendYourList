namespace BackendYourList.Models.Entities
{
    public class AssigneeModel
    {
        public int id { get; set; }
        public int projectid { get; set; }
        public string projectName { get; set; }
        public int customerid { get; set; }
        public string customerName { get; set; }
    }

    public class AssigneeRequestModel
    {
        public int projectid { get; set; }
        public int customerid { get; set; }
    }
    public class GetAssigneeModel
    {
        public int assigneeid { get; set; }
        public int projectid { get; set; }
        public string projectName { get; set; }
        public int customerid { get; set; }
        public string customerName { get; set; }
        public List<TaskModel> TaskModels { get; set; }
    }

    public class GetAssigneeByProjectModel
    {
        public int projectId { get; set; }
        public string projectName { get; set; }
        public List<GetAssigneeModel> getAssigneeModels { get; set; }
    }
}
