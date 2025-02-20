using BackendYourList.Models;
using BackendYourList.Models.Entities;
using BackendYourList.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BackendYourList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _service;

        public ProjectsController(IProjectsService service) 
        {
            this._service = service;
        }
        [HttpGet]
        public IActionResult GetAllProjects()
        {
            var result = _service.GetProjects();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetProjectById(int id)
        {
            var result = _service.GetProjectById(id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddProject(ProjectModel project)
        {
            var result = _service.AddProject(project);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateProject(int id, ProjectUpdateModel project)
        {
            var result = _service.UpdateProject(id, project);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteProject(int id)
        {
            var result = _service.DeleteProject(id);
            return Ok(result);
        }
        
    }
}
