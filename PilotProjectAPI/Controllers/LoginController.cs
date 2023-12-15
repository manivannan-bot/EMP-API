using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PilotProjectAPI.Data;
using PilotProjectAPI.Models;

namespace PilotProjectAPI.Controllers
{
    [Route("api/v1/users/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly ApplicationDBContext _dbContext;
        public LoginController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //[HttpGet("{id:int}")]
        //public ActionResult<User> GetById(int id)
        //{
        //    return _dbContext.Users.Find(id);
        //}

        //[HttpGet]
        //public ActionResult<IEnumerable<User>> getAll()
        //{
        //    return _dbContext.Users.ToList();
        //}


        [HttpGet("login")]
        public ActionResult<object> ValidateLogin([FromQuery] string login_id, [FromQuery] string password)
        {
            // Create parameters for the stored procedure
            var loginIdParam = new SqlParameter("@loginid", login_id);
            var passwordParam = new SqlParameter("@password", password);
            var isValidParam = new SqlParameter
            {
                ParameterName = "@IsValid",
                SqlDbType = System.Data.SqlDbType.Bit,
                Direction = System.Data.ParameterDirection.Output
            };

            _dbContext.Database.ExecuteSqlRaw("EXEC validate_login @loginid, @password, @IsValid OUTPUT", loginIdParam, passwordParam, isValidParam);

           
            var result = (bool)isValidParam.Value;

            if (result)
            {
                
                var user = _dbContext.Users
                    .Where(u => u.LoginId == login_id)
                    .Select(u => new { u.UserId, u.LoginId })
                    .FirstOrDefault();

                return Ok(user);
            }
            else
            {
               
                var errorResponse = new
                {
                    error_code = "InvalidCredentials",
                    error_message = "Invalid login ID or password."
                };
                return BadRequest(errorResponse);
            }
        }



    }
}
