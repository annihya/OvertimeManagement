using OvertimeManagement.Helper;
using OvertimeManagement.Models;
using OvertimeManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OvertimeManagement.Controllers
{
    public class OvertimesController : Controller
    {
        private readonly OvertimeManagementEntities db;

        public OvertimesController()
        {
            db = new OvertimeManagementEntities();
        }

        // GET: Overtimes
        public async Task<ActionResult> Index()
        {
            var overtimes = await OvertimesHelper.GetOvertimes();

            return View(overtimes ?? new List<ViewModel.OvertimeDisplayViewModel>());
        }

        // GET: Overtimes/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.EmployeeList = await EmployeesHelper.GetEmployeeList();
            ViewBag.IsEditMode = false;

            return PartialView("_OvertimeForm");
        }

        // POST: Overtimes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OvertimeDisplayViewModel model)
        {
            ViewBag.EmployeeList = await EmployeesHelper.GetEmployeeList();
            ViewBag.IsEditMode = false;

            if (!ModelState.IsValid)
            {
                var invalidFields = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => ms.Key)
                    .ToList();

                return Json(new
                {
                    success = false,
                    message = $"{string.Join(", ", invalidFields)} value is invalid!",
                });
            }

            try
            {
                // Check for duplicate overtime
                var calculatedHours = OvertimesHelper.CalculateOvertimeHours(model.TimeStart, model.TimeFinish);
                var overtime = new Overtime
                {
                    OvertimeID = Guid.NewGuid(),
                    EmployeeID = model.EmployeeID,
                    TimeStart = model.TimeStart,
                    TimeFinish = model.TimeFinish,
                    ActualHours = model.ActualHours,
                    CalculatedHours = model.CalculatedHours,
                    CreatedDate = DateTime.Now,
                };

                if (await OvertimesHelper.IsOvertimeDuplicate(overtime))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Duplicate overtime entry detected"
                    });
                }

                db.Overtimes.Add(overtime);
                await db.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Overtime created successfully.",
                    redirectUrl = Url.Action("Index", "Overtime")
                });
            }
            catch (Exception ex)
            {
                // Log the error (uncomment dex variable name and write a log.)
                return Json(new
                {
                    success = false,
                    message = "An error occurred while creating the overtime entry: " + ex.Message
                });
            }
        }

        // GET: Overtimes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var overtime = await OvertimesHelper.GetOvertimeById(id.Value);
            if (overtime == null)
            {
                return HttpNotFound();
            }

            ViewBag.EmployeeList = await EmployeesHelper.GetEmployeeList();
            ViewBag.IsEditMode = true;

            return PartialView("_OvertimeForm", overtime);
        }

        // POST: Overtimes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OvertimeDisplayViewModel model)
        {
            ViewBag.EmployeeList = await EmployeesHelper.GetEmployeeList();
            ViewBag.IsEditMode = true;

            if (!ModelState.IsValid)
            {
                var invalidFields = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => ms.Key)
                    .ToList();

                return Json(new
                {
                    success = false,
                    message = $"{string.Join(", ", invalidFields)} value is invalid!",
                });
            }

            try
            {
                var overtime = await db.Overtimes.FindAsync(model.OvertimeID);
                if (overtime == null)
                {
                    return HttpNotFound();
                }

                overtime.EmployeeID = model.EmployeeID;
                overtime.TimeStart = model.TimeStart;
                overtime.TimeFinish = model.TimeFinish;
                overtime.ActualHours = model.ActualHours;
                overtime.CalculatedHours = model.CalculatedHours;
                overtime.LastModifiedDate = DateTime.Now;

                if (await OvertimesHelper.IsOvertimeDuplicate(overtime))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Duplicate overtime entry detected",
                        redirectUrl = Url.Action("Index", "Overtime")
                    });
                }

                db.Entry(overtime).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Overtime edited successfully.",
                    redirectUrl = Url.Action("Index", "Overtime")
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while editing the overtime entry: " + ex.Message
                });
            }
        }

        // GET: Overtimes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Overtime overtime = await db.Overtimes.FindAsync(id);
            if (overtime == null)
            {
                return HttpNotFound();
            }
            return View(overtime);
        }

        // POST: Overtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Overtime overtime = await db.Overtimes.FindAsync(id);
            db.Overtimes.Remove(overtime);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
