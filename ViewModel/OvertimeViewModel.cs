using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OvertimeManagement.ViewModel
{
    public class OvertimeViewModel
    {
    }
    public class OvertimeDisplayViewModel
    {
        public Guid OvertimeID { get; set; }
        public Guid EmployeeID { get; set; }
        public string NIK { get; set; }
        public string FullName { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeFinish { get; set; }
        public double ActualHours { get; set; }
        public double CalculatedHours { get; set; }
    }
    public class OvertimeDisplayViewModelValidator : AbstractValidator<OvertimeDisplayViewModel>
    {
        public OvertimeDisplayViewModelValidator()
        {
            RuleFor(x => x.EmployeeID)
                .NotNull().WithMessage("Employee ID is required.");

            RuleFor(x => x.TimeStart)
                .NotNull().WithMessage("Time Start is required.");

            RuleFor(x => x.TimeFinish)
                .NotNull().WithMessage("Time Finish is required.");

            RuleFor(x => x.ActualHours)
                .NotNull().WithMessage("Actual Hours is required.");

            //RuleFor(x => x.AllowanceList)
            //    .NotNull().WithMessage("Please select at least one allowance.");
        }
    }
}