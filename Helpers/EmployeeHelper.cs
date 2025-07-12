using OvertimeManagement.Models;
using OvertimeManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static OvertimeManagement.Helper.Constants.GlobalContants;

namespace OvertimeManagement.Helper
{
    public static class EmployeeHelper
    {
        public static async Task<List<EmployeeDisplayViewModel>> GetEmployees(Guid? employeeID = null)
        {
            using (var db = new OvertimeManagementEntities())
            {
                var query = (from emp in db.Employees

                            join dept in db.Departments on emp.DepartmentID equals dept.DepartmentID into deptGroup
                            from dept in deptGroup.DefaultIfEmpty()

                            join pos in db.Positions on emp.PositionID equals pos.PositionID into posGroup
                            from pos in posGroup.DefaultIfEmpty()

                            select new EmployeeDisplayViewModel
                            {
                                EmployeeID = emp.EmployeeID,
                                NIK = emp.NIK,
                                FullName = emp.FullName,
                                DepartmentID = emp.DepartmentID,
                                DepartmentName = dept != null ? dept.DepartmentName : "N/A",
                                PositionID = emp.PositionID,
                                PositionName = pos != null ? pos.PositionName : "N/A",
                                Allowance = emp.Allowance,
                            });

                if (employeeID.HasValue)
                {
                    query = query.Where(e => e.EmployeeID == employeeID.Value);
                }

                return await query
                    .OrderBy(e => e.FullName)
                    .ToListAsync();
            }
        }
        public static async Task<EmployeeDisplayViewModel> GetEmployeeById(Guid employeeID)
        {
            var employees = await GetEmployees(employeeID);
            return employees.FirstOrDefault();
        }
        public static async Task<bool> IsNIKExists(string nik)
        {
            if (string.IsNullOrWhiteSpace(nik))
            {
                throw new ArgumentException("NIK cannot be null or empty.", nameof(nik));
            }
            
            using (var db = new OvertimeManagementEntities())
            {
                return await db.Employees.AnyAsync(e => e.NIK == nik);
            }
        }
        public static async Task<List<Department>> GetDepartments()
        {
            using (var db = new OvertimeManagementEntities())
            {
                return await db.Departments
                    .OrderBy(d => d.DepartmentName)
                    .ToListAsync();
            }
        }
        public static async Task<List<Position>> GetPositions()
        {
            using (var db = new OvertimeManagementEntities())
            {
                return await db.Positions
                    .OrderBy(d => d.PositionName)
                    .ToListAsync();
            }
        }
        public static List<AllowanceCheckViewModel> GetAllowanceList(string allowances = null)
        {
            var allowanceList = string.IsNullOrWhiteSpace(allowances) ? null : allowances.Split(',').ToList();

            return typeof(AllowanceType)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => (f.IsLiteral || f.IsInitOnly))
                .Select(f => new AllowanceCheckViewModel
                {
                    AllowanceType = f.GetRawConstantValue() != null ? f.GetRawConstantValue().ToString() : f.GetValue(null).ToString(),
                    IsChecked = false
                })
                .OrderBy(a => a.AllowanceType)
                .Select(a =>
                {
                    if (allowanceList != null && allowanceList.Contains(a.AllowanceType))
                    {
                        a.IsChecked = true;
                    }
                    return a;
                })
                .ToList();
        }
    }
}