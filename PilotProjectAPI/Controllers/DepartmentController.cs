using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PilotProjectAPI.Data;
using PilotProjectAPI.Models;

namespace PilotProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        public readonly ApplicationDBContext _dbContext;
        public DepartmentController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Department>> getAll()
        {
            return _dbContext.Departments.ToList();
        }
    }
}
