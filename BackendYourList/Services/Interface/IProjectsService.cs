using BackendYourList.Models.Entities;
using BackendYourList.Models;

namespace BackendYourList.Services.Interface
{
    public interface IProjectsService
    {
        ResultModel GetProjects();
        ResultModel GetProjectById(int id);
        ResultModel AddProject(ProjectModel project);
        ResultModel UpdateProject(int id, ProjectUpdateModel project);
        ResultModel DeleteProject(int id);
    }
}
