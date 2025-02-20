using BackendYourList.Models;

namespace BackendYourList.Services.Interface
{
    public interface ITasksService
    {
        ResultModel GetTasks();
        ResultModel GetTaskById(int id);
        ResultModel AddTask(TaskModel model);
        ResultModel UpdateTask(int id, TaskrUpdateModel model);
        ResultModel DeleteTask(int id);
    }
}
