namespace PilotProjectAPI.Models
{
    public class EmployeeEditRequestModel
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public int DepartmentId { get; set; }
        public DateTime HiredDate { get; set; }
    }

}
