using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PilotProjectAPI.Data;
using PilotProjectAPI.Models;

namespace PilotProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        public readonly ApplicationDBContext _dbContext;
        public MemberController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<Member>> getAll() {
        //    return _dbContext.Members.ToList();
        //}
        //[HttpPost]
        //public ActionResult<Member> Create([FromBody] Member member) {
        //_dbContext.Members.Add(member);
        //    _dbContext.SaveChanges();
        //    return Ok();
        // }
    }
}
