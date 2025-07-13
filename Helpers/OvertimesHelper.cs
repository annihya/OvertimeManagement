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
    public static class OvertimesHelper
    {
        public static async Task<List<OvertimeDisplayViewModel>> GetOvertimes(Guid? overtimeID = null)
        {
            using (var db = new OvertimeManagementEntities())
            {
                var query = (from ovt in db.Overtimes

                             join emp in db.Employees on ovt.EmployeeID equals emp.EmployeeID

                             select new OvertimeDisplayViewModel
                             {
                                 OvertimeID = ovt.OvertimeID,
                                 EmployeeID = emp.EmployeeID,
                                 NIK = emp.NIK,
                                 FullName = emp.FullName,
                                 TimeStart = ovt.TimeStart,
                                 TimeFinish = ovt.TimeFinish,
                                 ActualHours = ovt.ActualHours,
                                 CalculatedHours = ovt.CalculatedHours,
                             });

                if (overtimeID.HasValue)
                {
                    query = query.Where(e => e.OvertimeID == overtimeID.Value);
                }

                return await query
                    .OrderBy(e => e.FullName)
                    .ToListAsync();
            }
        }
        public static async Task<OvertimeDisplayViewModel> GetOvertimeById(Guid overtimeID)
        {
            var overtime = await GetOvertimes(overtimeID);
            return overtime.FirstOrDefault();
        }
        public static int CalculateOvertimeHours(DateTime timeStart, DateTime timeFinish)
        {
            // Calculate the total hours worked
            var totalHours = (timeFinish - timeStart).TotalHours;
            // Round to two decimal places
            return (int)Math.Round(totalHours, 2);
        }
        public static async Task<bool> IsOvertimeDuplicate(Overtime overtime)
        {
            using (var db = new OvertimeManagementEntities())
            {
                return await db.Overtimes
                    .AnyAsync(e => 
                        e.EmployeeID == overtime.EmployeeID
                        && e.TimeStart == overtime.TimeStart
                        && e.TimeFinish == overtime.TimeFinish
                        && e.OvertimeID != overtime.OvertimeID);
            }
        }
    }
}