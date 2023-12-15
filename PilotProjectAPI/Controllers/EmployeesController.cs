using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PilotProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;

[Route("api/")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public EmployeesController(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    [HttpGet("employees")]
    public IActionResult GetEmployees(string employee_unique_id = null, string department_id = null, int age_from = 0, int age_to = 0, string hired_date_from = null, string hired_date_to = null)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("get_employee_list", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@employee_unique_id", string.IsNullOrEmpty(employee_unique_id) ? (object)DBNull.Value : employee_unique_id);
                    command.Parameters.AddWithValue("@departmentid", string.IsNullOrEmpty(department_id) ? (object)DBNull.Value : department_id);
                    command.Parameters.AddWithValue("@age_from", age_from);
                    command.Parameters.AddWithValue("@age_to", age_to);
                    command.Parameters.AddWithValue("@hired_date_from", string.IsNullOrEmpty(hired_date_from) ? (object)DBNull.Value : hired_date_from);
                    command.Parameters.AddWithValue("@hired_date_to", string.IsNullOrEmpty(hired_date_to) ? (object)DBNull.Value : hired_date_to);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<EmployeeViewModel> employees = new List<EmployeeViewModel>();

                        while (reader.Read())
                        {
                            EmployeeViewModel employee = new EmployeeViewModel
                            {
                                EmployeeUniqueId = reader.GetInt32("employee_unique_id"),
                                EmployeeId = reader.GetString("employee_id"),
                                Name = reader.GetString("name"),
                                Gender = reader.GetString("gender"),
                                Dob = reader.GetDateTime("dob"),
                                DepartmentId = reader.GetInt32("departmentid"),
                                DepartmentName = reader.GetString("department_name"),
                                HiredDate = reader.GetDateTime("hired_date"),
                                Contacts = new List<EmployeeContactViewModel>()
                            };

                            employees.Add(employee);
                        }

                        reader.NextResult();

                        while (reader.Read())
                        {
                            EmployeeContactViewModel contact = new EmployeeContactViewModel
                            {
                                EmployeeUniqueId = reader.GetInt32("employee_unique_id"),
                                EmployeeContactId = reader.GetInt32("employee_contact_id"),
                                Name = reader.GetString("name"),
                                Relation = reader.GetString("relation"),
                                Email = reader.GetString("email"),
                                Phone = reader.GetString("phone"),
                                City = reader.GetString("city"),
                                Zip = reader.GetString("zip"),
                                Address = reader.GetString("address"),
                            };

                            int employeeUniqueId = contact.EmployeeUniqueId;

                            EmployeeViewModel employee = employees.Find(e => e.EmployeeUniqueId == employeeUniqueId);
                            if (employee != null)
                            {
                                employee.Contacts.Add(contact);
                            }
                        }

                        return Ok(employees);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var errorResponse = new
            {
                error_code = "INVALID_PARAMETER",
                error_message =  ex.Message
        };
            return StatusCode(400, errorResponse);
        }
    }




    [HttpPost("employees")]
    public IActionResult AddEmployee([FromBody] EmployeeRequestModel employeeRequest)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("add_employee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@employee_id", employeeRequest.EmployeeId);
                    command.Parameters.AddWithValue("@name", employeeRequest.Name);
                    command.Parameters.AddWithValue("@gender", employeeRequest.Gender);
                    command.Parameters.AddWithValue("@dob", employeeRequest.Dob);
                    command.Parameters.AddWithValue("@departmentid", employeeRequest.DepartmentId);
                    command.Parameters.AddWithValue("@hired_date", employeeRequest.HiredDate);

                    command.Parameters.Add("@errors", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    SqlParameter newEmployeeUniqueIdParam = command.Parameters.Add("@new_employee_unique_id", SqlDbType.Int);
                    newEmployeeUniqueIdParam.Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    // Check for errors and handle accordingly
                    string errors = command.Parameters["@errors"].Value.ToString();
                    if (!string.IsNullOrEmpty(errors))
                    {
                        var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = errors };
                        return StatusCode(400, errorResponse);
                    }

                    // Retrieve the value of the output parameter
                    int newEmployeeUniqueId = (int)newEmployeeUniqueIdParam.Value;

                    return StatusCode(201,new { new_employee_unique_id = newEmployeeUniqueId });
                }
            }
        }
        catch (Exception ex)
        {
            var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = ex.Message };
            return StatusCode(400, errorResponse);
        }
    }

    [HttpPut("employees/{employee_unique_id}")]
    public IActionResult EditEmployee(int employee_unique_id, [FromBody] EmployeeEditRequestModel employeeEditRequest)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("edit_employee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@employee_unique_id", employee_unique_id);
                    command.Parameters.AddWithValue("@employee_id", employeeEditRequest.EmployeeId);
                    command.Parameters.AddWithValue("@name", employeeEditRequest.Name);
                    command.Parameters.AddWithValue("@gender", employeeEditRequest.Gender);
                    command.Parameters.AddWithValue("@dob", employeeEditRequest.Dob);
                    command.Parameters.AddWithValue("@departmentid", employeeEditRequest.DepartmentId);
                    command.Parameters.AddWithValue("@hired_date", employeeEditRequest.HiredDate);

                    command.Parameters.Add("@errors", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                    int result = command.ExecuteNonQuery();

                    // Check for errors and handle accordingly
                    string errors = command.Parameters["@errors"].Value.ToString();
                    if (!string.IsNullOrEmpty(errors))
                    {
                        var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = errors };
                        return StatusCode(400, errorResponse);
                    }

                    return Ok(new { message = "Employee edited successfully" });
                }
            }
        }
        catch (Exception ex)
        {
            var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = ex.Message };
            return StatusCode(400, errorResponse);
        }
    }

    [HttpDelete("employees/{employee_unique_id}")]
    public IActionResult DeleteEmployee(int employee_unique_id)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("delete_employee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@employee_unique_id", employee_unique_id);
                    command.Parameters.Add("@errors", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                    int result = command.ExecuteNonQuery();

                    // Check for errors and handle accordingly
                    string errors = command.Parameters["@errors"].Value.ToString();
                    if (!string.IsNullOrEmpty(errors))
                    {
                        var errorResponse = new { error_code = "INVALID_PARAMETER", error_message = errors };
                        return StatusCode(400, errorResponse);
                    }

                    return Ok(new { message = "Employee deleted successfully" });
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
