using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PilotProjectAPI.Data;
using PilotProjectAPI.Models;
using System.Data;

namespace PilotProjectAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EmployeeContactController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public EmployeeContactController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost("employees/{employee_unique_id}/contacts")]
        public IActionResult AddEmployeeContact(int employee_unique_id, [FromBody] EmployeeContactRequestModel contactRequest)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("add_employee_contact", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@employee_unique_id", employee_unique_id);
                        command.Parameters.AddWithValue("@name", contactRequest.Name);
                        command.Parameters.AddWithValue("@relation", contactRequest.Relation);
                        command.Parameters.AddWithValue("@email", contactRequest.Email);
                        command.Parameters.AddWithValue("@phone", contactRequest.Phone);
                        command.Parameters.AddWithValue("@city", contactRequest.City);
                        command.Parameters.AddWithValue("@zip", contactRequest.Zip);
                        command.Parameters.AddWithValue("@address", contactRequest.Address);

                        command.Parameters.Add("@success", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@errors", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        bool addContactSuccess = Convert.ToBoolean(command.Parameters["@success"].Value);
                        string errors = command.Parameters["@errors"].Value.ToString();

                        if (addContactSuccess)
                        {
                            return Ok(new { message = "Employee Contact added successfully" });
                        }
                        else
                        {
                            var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = errors };
                            return StatusCode(400, errorResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = ex.Message };
                return StatusCode(400, errorResponse);
            }
        }

        [HttpPut("employees/{employee_unique_id}/contacts/{employee_contact_id}")]
        public IActionResult EditEmployeeContact(int employee_unique_id, int employee_contact_id, [FromBody] EmployeeContactRequestModel contactRequest)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("edit_employee_contact", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@employee_contact_id", employee_contact_id);
                        command.Parameters.AddWithValue("@name", contactRequest.Name);
                        command.Parameters.AddWithValue("@relation", contactRequest.Relation);
                        command.Parameters.AddWithValue("@email", contactRequest.Email);
                        command.Parameters.AddWithValue("@phone", contactRequest.Phone);
                        command.Parameters.AddWithValue("@city", contactRequest.City);
                        command.Parameters.AddWithValue("@zip", contactRequest.Zip);
                        command.Parameters.AddWithValue("@address", contactRequest.Address);

                        command.Parameters.Add("@errors", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        string errors = command.Parameters["@errors"].Value.ToString();

                        if (string.IsNullOrEmpty(errors))
                        {
                            return Ok(new { message = "Employee Contact edited successfully" });
                        }
                        else
                        {
                            var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = errors };
                            return StatusCode(400, errorResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = ex.Message };
                return StatusCode(400, errorResponse);
            }
        }


        [HttpDelete("employees/{employee_unique_id}/contacts/{employee_contact_id}")]
        public IActionResult DeleteEmployeeContact(int employee_unique_id, int employee_contact_id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("delete_employee_contact", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@employee_contact_id", employee_contact_id);
                        command.Parameters.Add("@errors", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        string errors = command.Parameters["@errors"].Value.ToString();

                        if (string.IsNullOrEmpty(errors))
                        {
                            return Ok(new { message = "Employee Contact deleted successfully" });
                        }
                        else
                        {
                            var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = errors };
                            return StatusCode(400, errorResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = ex.Message };
                return StatusCode(400, errorResponse);
            }
        }


    }
}
