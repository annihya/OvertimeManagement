using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OvertimeManagement.Models;
using OvertimeManagement.ViewModel;
using OvertimeManagement.Helper;

namespace OvertimeManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly OvertimeManagementEntities db;

        public EmployeesController()
        {
            db = new OvertimeManagementEntities();
        }

        // GET: Employees
        public async Task<ActionResult> Index()
        {
            var employees = await EmployeeHelper.GetEmployees();

            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.DepartmentList = await EmployeeHelper.GetDepartments(); 
            ViewBag.PositionList = await EmployeeHelper.GetPositions();
            ViewBag.AllowanceList = EmployeeHelper.GetAllowanceList();
            ViewBag.IsEditMode = false;

            return PartialView("_EmployeeForm");
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmployeeDisplayViewModel model)
        {
            ViewBag.DepartmentList = await EmployeeHelper.GetDepartments();
            ViewBag.PositionList = await EmployeeHelper.GetPositions();
            ViewBag.AllowanceList = EmployeeHelper.GetAllowanceList(model.Allowance);
            ViewBag.IsEditMode = false;

            if (!ModelState.IsValid)
            {
                // Validate error → re-render the PartialView with error messages
                return PartialView("_EmployeeForm", model); 
            }

            try
            {
                // Check if NIK already exists
                if (await EmployeeHelper.IsNIKExists(model.NIK))
                {
                    return Json(new
                    {
                        success = false,
                        message = "The NIK is already in use by another employee."
                    });
                }

                var employee = new Employee
                {
                    EmployeeID = Guid.NewGuid(),
                    NIK = model.NIK,
                    FullName = model.FullName,
                    Allowance = string.Join(",", model.AllowanceList.Where(a => a.IsChecked).Select(a => a.AllowanceType)),
                    DepartmentID = model.DepartmentID,
                    PositionID = model.PositionID
                };

                db.Employees.Add(employee);
                await db.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Employee created successfully.",
                    redirectUrl = Url.Action("Index", "Employee")
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the employee: " + ex.Message);
                return PartialView("_EmployeeForm", model);
            }
        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var employee = await EmployeeHelper.GetEmployeeById(id.Value);
            if (employee == null)
            {
                return HttpNotFound();
            }

            ViewBag.DepartmentList = new SelectList(db.Departments, "DepartmentID", "Name", employee.DepartmentID);
            ViewBag.PositionList = new SelectList(db.Positions, "PositionID", "Name", employee.PositionID);
            ViewBag.AllowanceList = EmployeeHelper.GetAllowanceList(employee.Allowance);
            ViewBag.IsEditMode = true;

            return PartialView("_EmployeeForm", employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EmployeeDisplayViewModel model)
        {
            ViewBag.DepartmentList = await EmployeeHelper.GetDepartments();
            ViewBag.PositionList = await EmployeeHelper.GetPositions();
            ViewBag.AllowanceList = EmployeeHelper.GetAllowanceList(model.Allowance);
            ViewBag.IsEditMode = true;

            if (!ModelState.IsValid)
            {
                // Validate error → re-render the PartialView with error messages
                return PartialView("_EmployeeForm", model);
            }

            try
            {
                var employee = await db.Employees.FindAsync(model.EmployeeID);
                if (employee == null)
                {
                    return HttpNotFound();
                }

                employee.NIK = model.NIK;
                employee.FullName = model.FullName;
                employee.Allowance = string.Join(",", model.AllowanceList.Where(a => a.IsChecked).Select(a => a.AllowanceType));
                employee.DepartmentID = model.DepartmentID;
                employee.PositionID = model.PositionID;

                db.Entry(employee).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Employee created successfully.",
                    redirectUrl = Url.Action("Index", "Employee")
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the employee: " + ex.Message);
                return PartialView("_EmployeeForm", model);
            }
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            db.Employees.Remove(employee);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> CheckNIKAvailability(string nik)
        {
            try
            {
                var exists = await EmployeeHelper.IsNIKExists(nik);
                return Json(!exists, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while checking NIK availability: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
