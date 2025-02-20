using BackendYourList.Models;
using BackendYourList.Models.Entities;
using BackendYourList.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BackendYourList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssigneesControllers : ControllerBase
    {
        private readonly IAssigneesService _service;

        public AssigneesControllers(IAssigneesService service) 
        {
            this._service = service;
        }
        [HttpGet]
        public IActionResult GetAllAssignees()
        {
            var result = _service.GetAssignees();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetAssigneeById(int id)
        {
            var result = _service.GetAssigneeById(id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddAssignee(AssigneeRequestModel model)
        {
            var result = _service.AddAssignee(model);
            return Ok(result);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateAssignee(int id, AssigneeModel model)
        {
            var result = _service.UpdateAssignee(id, model);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteAssignee(int id)
        {
            var result = _service.DeleteAssignee(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAssigneeByProject/{id:int}")]
        public IActionResult GetAssigneeByProject(int id)
        {
            var result = _service.GetAssigneeByProject(id);
            return Ok(result);
        }
    }
}
