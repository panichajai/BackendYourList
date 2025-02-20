using BackendYourList.Models.Entities;
using BackendYourList.Models;

namespace BackendYourList.Services.Interface
{
    public interface IAssigneesService
    {
        ResultModel GetAssignees();
        ResultModel GetAssigneeById(int id);
        ResultModel AddAssignee(AssigneeRequestModel model);
        ResultModel UpdateAssignee(int id, AssigneeModel model);
        ResultModel DeleteAssignee(int id);
        public ResultModel GetAssigneeByProject(int id);

    }
}
