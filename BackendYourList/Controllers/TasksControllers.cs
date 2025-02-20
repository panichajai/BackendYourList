using BackendYourList.Models.Entities;
using BackendYourList.Models;
using BackendYourList.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BackendYourList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksControllers : ControllerBase
    {
        private readonly ITasksService _service;

        public TasksControllers(ITasksService service)
        {
            this._service = service;
        }
        [HttpGet]
        public IActionResult GetAllTasks()
        {
            var result = _service.GetTasks();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetTaskById(int id)
        {
            var result = _service.GetTaskById(id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddTask(TaskModel task)
        {
            var result = _service.AddTask(task);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateTask(int id, TaskrUpdateModel model)
        {
            var result = _service.UpdateTask(id, model);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteTask(int id)
        {
            var result = _service.DeleteTask(id);
            return Ok(result);
        }
    }
}
