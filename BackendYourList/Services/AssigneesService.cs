using BackendYourList.Data;
using BackendYourList.Models;
using BackendYourList.Models.Entities;
using BackendYourList.Services.Interface;

namespace BackendYourList.Services
{
    public class AssigneesService : IAssigneesService
    {
        private readonly ApplicationDbContext _dbContext;

        public AssigneesService(ApplicationDbContext dbContext) 
        {
            this._dbContext = dbContext;
        }
        public ResultModel AddAssignee(AssigneeRequestModel model)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var customer = _dbContext.customers.Where(c => c.id == model.customerid).FirstOrDefault();
                if (customer == null)
                {
                    result.success = false;
                    result.message = "An assignee with this name already exists." + customer.id;
                    result.status = 501;
                    return result;
                }
                var assigneeEntity = new tb_Assignee()
                {
                    //name = model.name,
                    customerid = model.customerid,
                    projectid = model.projectid,
                    createBy = "admin",
                    createDate = DateTime.Now,
                    updateBy = "admin",
                    updateDate = DateTime.Now,
                    isDelete = false,
                };

                _dbContext.assignees.Add(assigneeEntity);
                _dbContext.SaveChanges();
                result.data = assigneeEntity;
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

        public ResultModel DeleteAssignee(int id)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var assignee = _dbContext.assignees.Find(id);

                if (assignee is null)
                {
                    result.success = false;
                    result.message = "Not Have Assignee ById : " + id;
                    result.status = 501;
                    return result;
                }

                _dbContext.assignees.Remove(assignee);
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

        public ResultModel GetAssigneeById(int id)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var assignee = _dbContext.assignees.Find(id);
                if (assignee is null)
                {
                    result.success = false;
                    result.message = "Not Have Assignee ById : " + id;
                    result.status = 501;
                    return result;
                }
                result.data = assignee;
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

        public ResultModel GetAssignees()
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var assignees = _dbContext.assignees.Select(s => new AssigneeModel
                {
                    id = s.id,
                    customerid = s.customerid,
                    customerName = s.customer.fname + " " + s.customer.lname,
                    projectid = s.projectid,
                    projectName = s.project.name
                }).ToList();
                result.data = assignees;
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

        public ResultModel UpdateAssignee(int id, AssigneeModel model)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var Assignee = _dbContext.assignees.Find(id);

                if (Assignee is null)
                {
                    result.success = false;
                    result.message = "Not Have Assignee ById : " + id;
                    result.status = 501;
                    return result;
                }
                Assignee.customerid = model.customerid;
                Assignee.updateDate = DateTime.Now;
                Assignee.updateBy = "admin";


                _dbContext.SaveChanges();
                result.data = model;
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

        public ResultModel GetAssigneeByProject(int id)
            {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                string projectName = _dbContext.projects.Where(w => w.id == id).Select(s => s.name).FirstOrDefault();

                var assignees = _dbContext.assignees.Where(w => w.projectid == id)
                    .Select(s => new GetAssigneeModel
                    {
                        assigneeid = s.id,
                        customerid = s.customerid,
                        customerName = s.customer.fname + " " + s.customer.lname,
                        projectid = s.projectid,
                        projectName = s.project.name,
                        TaskModels = _dbContext.tasks
                        .Where(t => t.assigneeid == s.id && !t.isDelete)
                        .Select(ss => new TaskModel
            {
                            id = ss.id,
                            name = ss.name,
                            description = ss.description,
                            startdate = ss.startdate,
                            enddate = ss.enddate,
                            status = ss.status,
                            assigneeid = ss.assigneeid,
                            createBy = ss.createBy,
                            updateBy = ss.updateBy,
                        }).ToList()
                    }).ToList();

                if (assignees is null)
                {
                    result.success = false;
                    result.message = "Not Have Assignee By Project ID : " + id;
                    result.status = 501;
                    return result;
                }

                GetAssigneeByProjectModel getAssigneeByProjectModel = new GetAssigneeByProjectModel();
                getAssigneeByProjectModel.projectId = id;
                getAssigneeByProjectModel.projectName = projectName;
                getAssigneeByProjectModel.getAssigneeModels = assignees;
                result.data = getAssigneeByProjectModel;
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
