using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace OvertimeManagement.ViewModel
{
    public class EmployeeViewModel
    {
        
    }
    public class EmployeeDisplayViewModel
    {
        public Guid EmployeeID { get; set; }
        public string NIK { get; set; }
        public string FullName { get; set; }
        public Guid DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Guid PositionID { get; set; }
        public string PositionName { get; set; }
        public string Allowance { get; set; }
        public List<AllowanceCheckViewModel> AllowanceList { get; set; }
    }
    public class AllowanceCheckViewModel
    {
        public string AllowanceType { get; set; }
        public bool IsChecked { get; set; }
    }
    public class EmployeeDropdown
    {
        public Guid EmployeeID { get; set; }
        public string EmployeeInfo { get; set; }
    }
    public class EmployeeDisplayViewModelValidator : AbstractValidator<EmployeeDisplayViewModel>
    {
        public EmployeeDisplayViewModelValidator()
        {
            RuleFor(x => x.NIK)
                .NotEmpty().WithMessage("NIK is required.")
                .Length(5, 20).WithMessage("NIK must be between 5 and 20 characters.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.");

            RuleFor(x => x.DepartmentID)
                .NotNull().WithMessage("Please select a department.");

            RuleFor(x => x.PositionID)
                .NotNull().WithMessage("Please select a position.");

            //RuleFor(x => x.AllowanceList)
            //    .NotNull().WithMessage("Please select at least one allowance.");
        }
    }

}