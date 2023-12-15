namespace PilotProjectAPI.Models
{

    public class EmployeeViewModel
    {
        public int EmployeeUniqueId { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public DateTime HiredDate { get; set; }
        public List<EmployeeContactViewModel> Contacts { get; set; }
    }

    public class EmployeeContactViewModel
    {
        public int EmployeeUniqueId { get; set; }
        public int EmployeeContactId { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
    }



}
