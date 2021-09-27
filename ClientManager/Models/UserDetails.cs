using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientManager.Models
{
    public class UserRoleData
    {
        public int UserId { get; set; }
        public int[] SelectedRoles { get; set; }        
    }
    public class UserDetails
    {
        public int Id { get; set; }        
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public string FullName { get; set; }
        public int? ReportingManager { get; set; }
        public List<int> ReportingToMe { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }

    public class UserData
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string EmpId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateofJoining { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public int? SaleTarget { get; set; }
        public int? ReportingManager { get; set; }        
    }

    public class UserRole
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}