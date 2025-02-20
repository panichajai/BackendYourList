using System.Threading.Tasks;
using BackendYourList.Data;
using BackendYourList.Models;
using BackendYourList.Models.Entities;
using BackendYourList.Services.Interface;

namespace BackendYourList.Services
{
    public class TasksService : ITasksService
    {
        private readonly ApplicationDbContext _dbContext;

        public TasksService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public ResultModel AddTask(TaskModel model)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var taskEntity = new tb_Task
                {
                    name = model.name,
                    description = model.description,
                    startdate = model.startdate,
                    enddate = model.enddate,
                    assigneeid = model.assigneeid,
                    status = model.status,
                    createBy = model.createBy,
                    updateBy = model.updateBy,
                    createDate = DateTime.Now,
                    updateDate = DateTime.Now,
                    isDelete = false,
                };

                _dbContext.tasks.Add(taskEntity);
                _dbContext.SaveChanges();
                result.data = taskEntity;
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

        public ResultModel DeleteTask(int id)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var task = _dbContext.tasks.Find(id);

                if (task is null)
                {
                    result.success = false;
                    result.message = "Not Have Task ById : " + id;
                    result.status = 501;
                    return result;
                }

                task.isDelete = true;
                _dbContext.tasks.Update(task);
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

        //public ResultModel GetTaskById(int id)
        //{
        //    var result = new ResultModel
        //    {
        //        status = 200,
        //        success = true,
        //    };
        //    try
        //    {
        //        var task = _dbContext.tasks.Find(id);
        //        if (task is null)
        //        {
        //            result.success = false;
        //            result.message = "Not Have Task ById : " + id;
        //            result.status = 501;
        //            return result;
        //        }
        //        if (DateTime.Now > task.enddate && task.status != "Completed")
        //        {
        //            task.status = "Overdue";
        //            _dbContext.tasks.Update(task);
        //            _dbContext.SaveChanges();
        //        }

        //        result.data = new TaskModel
        //        {
        //            id = task.id,
        //            name = task.name,
        //            description = task.description,
        //            startdate = task.startdate,
        //            enddate = task.enddate,
        //            status = task.status,
        //            assigneeid = task.assigneeid,
        //            createBy = task.createBy,
        //            updateBy = task.updateBy,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultModel
        //        {
        //            status = 500,
        //            success = false,
        //            message = ex.Message
        //        };
        //    }
        //    return result;
        //}

        //public ResultModel GetTasks()
        //{
        //    var result = new ResultModel
        //    {
        //        status = 200,
        //        success = true,
        //    };
        //    try
        //    {
        //        var tasks = _dbContext.tasks
        //            .Where(s => !s.isDelete)
        //            .Select(s => new TaskModel
        //        {
        //            id = s.id,
        //            name = s.name,
        //            description = s.description,
        //            startdate = s.startdate,
        //            enddate = s.enddate,
        //            //status = s.status,
        //            status = DateTime.Now > s.enddate && s.status != "Completed" ? "Overdue" : s.status,
        //            assigneeid = s.assigneeid,
        //            createBy = s.createBy,
        //            updateBy = s.updateBy,
        //            }).ToList();
        //        result.data = tasks;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultModel
        //        {
        //            status = 500,
        //            success = false,
        //            message = ex.Message
        //        };
        //    }
        //    return result;
        //}
        public ResultModel GetTasks()
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var overdueTasks = _dbContext.tasks
                    .Where(s => !s.isDelete && DateTime.Now > s.enddate && s.status != "Completed")
                    .ToList();

                foreach (var task in overdueTasks)
                {
                    task.status = "Overdue";
                }

                if (overdueTasks.Any())
                {
                    _dbContext.SaveChanges();
                }

                var tasks = _dbContext.tasks
                    .Where(s => !s.isDelete)
                    .Select(s => new TaskModel
                    {
                        id = s.id,
                        name = s.name,
                        description = s.description,
                        startdate = s.startdate,
                        enddate = s.enddate,
                        status = s.status,
                        assigneeid = s.assigneeid,
                        createBy = s.createBy,
                        updateBy = s.updateBy,
                    }).ToList();

                result.data = tasks;
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

        public ResultModel GetTaskById(int id)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var task = _dbContext.tasks.Find(id);
                if (task is null)
                {
                    result.success = false;
                    result.message = "Not Have Task ById : " + id;
                    result.status = 404;
                    return result;
                }

                if (DateTime.Now > task.enddate && task.status != "Completed")
                {
                    task.status = "Overdue";
                    _dbContext.tasks.Update(task);
                    _dbContext.SaveChanges();
                }

                result.data = new TaskModel
                {
                    id = task.id,
                    name = task.name,
                    description = task.description,
                    startdate = task.startdate,
                    enddate = task.enddate,
                    status = task.status,
                    assigneeid = task.assigneeid,
                    createBy = task.createBy,
                    updateBy = task.updateBy,
                };
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

        public ResultModel UpdateTask(int id, TaskrUpdateModel model)
        {
            var result = new ResultModel
            {
                status = 200,
                success = true,
            };
            try
            {
                var Task = _dbContext.tasks.Find(id);

                if (Task is null)
                {
                    result.success = false;
                    result.message = "Not Have Task ById : " + id;
                    result.status = 501;
                    return result;
                }
                Task.name = model.name;
                Task.description = model.description;
                Task.startdate = model.startdate; 
                Task.enddate = model.enddate;
                Task.status = model.status;
                Task.updateBy = model.updateBy;
                Task.updateDate = DateTime.Now;

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
    }
}
