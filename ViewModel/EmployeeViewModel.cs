using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OvertimeManagement.ViewModel
{
    public class EmployeeViewModel
    {
        public class EmployeeDisplayModel
        {
            public Guid EmployeeID { get; set; }
            public string NIK { get; set; }
            public string FullName { get; set; }
            public string DepartmentName { get; set; }
            public string PositionName { get; set; }
        }
    }
}