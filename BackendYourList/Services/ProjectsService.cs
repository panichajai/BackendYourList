using System.Threading.Tasks;
using BackendYourList.Data;
using BackendYourList.Models;
using BackendYourList.Models.Entities;
using BackendYourList.Services.Interface;

namespace BackendYourList.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectsService(ApplicationDbContext dbContext) 
        {
            this._dbContext = dbContext;
        }

        public ResultModel AddProject(ProjectModel project)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var existingProject = _dbContext.projects.FirstOrDefault(n => n.name == project.name);
                if (existingProject != null)
                {
                    result.success = false;
                    result.message = "An project with this name already exists." + existingProject.name;
                    result.status = 501;
                    return result;
                }
                var projectEntity = new tb_Project()
                {
                    name = project.name,
                    details = project.details,
                    createBy = project.createBy,
                    updateBy = project.updateBy,
                    createDate = DateTime.Now,
                    updateDate = DateTime.Now,
                    isDelete = false,
                };

                _dbContext.projects.Add(projectEntity);
                _dbContext.SaveChanges();
                result.data = projectEntity;
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    status = 500,
                    success = false,
                    message = ex.Message
                };
            }
            return result;
        }

        public ResultModel DeleteProject(int id)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var project = _dbContext.projects.Find(id);

                if (project is null)
                {
                    result.success = false;
                    result.message = "Not Have Project ById : " + id;
                    result.status = 501;
                    return result;
                }
                project.isDelete = true;
                _dbContext.projects.Update(project);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    status = 500,
                    success = false,
                    message = ex.Message
                };
            }
            return result;
        }

        public ResultModel GetProjectById(int id)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var project = _dbContext.projects
                    .Where(p => p.id == id && !p.isDelete)
                    .Select(p => new ProjectModel
                    {
                        id = p.id,
                        name = p.name,
                        details = p.details,
                        createBy = p.createBy,
                        updateBy = p.updateBy,
                    })
                    .FirstOrDefault();

                if (project is null)
                {
                    result.success = false;
                    result.message = "Not Have Project ById : " + id;
                    result.status = 501;
                    return result;
                }
                result.data = project;
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    status = 500,
                    success = false,
                    message = ex.Message
                };
            }
            return result;
        }

        public ResultModel GetProjects()
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var projects = _dbContext.projects
                    .Where(s => !s.isDelete)
                    .Select(s => new ProjectModel
                {
                    id = s.id,
                    name = s.name,
                    details = s.details,
                    createBy = s.createBy,
                    updateBy = s.updateBy,
                    }).ToList();
                result.data = projects;
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    status = 500,
                    success = false,
                    message = ex.Message
                };
            }
            return result;
        }

        public ResultModel UpdateProject(int id, ProjectUpdateModel project)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var Project = _dbContext.projects.FirstOrDefault(p => p.id == id && !p.isDelete);

                if (Project is null)
                {
                    result.success = false;
                    result.message = "Not Have Project ById : " + id;
                    result.status = 501;
                    return result;
                }
                Project.name = project.name;
                Project.details = project.details;
                Project.updateBy = project.updateBy;
                Project.updateDate = DateTime.Now;

                _dbContext.SaveChanges();
                result.data = project;
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    status = 500,
                    success = false,
                    message = ex.Message
                };
            }
            return result;
        }
    }
}
