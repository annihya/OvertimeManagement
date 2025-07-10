using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OvertimeManagement.Models;
using OvertimeManagement.ViewModel;
using static OvertimeManagement.ViewModel.EmployeeViewModel;

namespace OvertimeManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private DBModel db;

        public EmployeesController()
        {
            db = new DBModel();
        }

        // GET: Employees
        public async Task<ActionResult> Index()
        {
            var employees = await (from emp in db.Employees

                                   join dept in db.Departments on emp.DepartmentID equals dept.DepartmentID into deptGroup
                                   from dept in deptGroup.DefaultIfEmpty()

                                   join pos in db.Positions on emp.PositionID equals pos.PositionID into posGroup
                                   from pos in posGroup.DefaultIfEmpty()
                                   
                                   select new EmployeeDisplayModel
                                   {
                                       EmployeeID = emp.EmployeeID,
                                       NIK = emp.NIK,
                                       FullName = emp.FullName,
                                       DepartmentName = dept != null ? dept.DepartmentName : "N/A",
                                       PositionName = pos != null ? pos.PositionName : "N/A"
                                   })
                                   .ToListAsync();

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
            var departmentList = await db.Departments.ToListAsync();
            var positionList = await db.Positions.ToListAsync();

            ViewBag.PositionList = new SelectList(positionList, "PositionID", "PositionName");
            ViewBag.DepartmentList = new SelectList(departmentList, "DepartmentID", "DepartmentName");

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeID,NIK,FullName,Allowance,DepartmentID,PositionID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                // Check if NIK already exists
                if (await IsNIKExists(employee.NIK))
                {
                    ModelState.AddModelError("NIK", "NIK is already in use.");
                    return View(employee);
                }

                employee.EmployeeID = Guid.NewGuid();
                db.Employees.Add(employee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
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

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,NIK,FullName,Allowance,DepartmentID,PositionID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
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

        // AJAX: Check if NIK is available
        [HttpPost]
        public async Task<JsonResult> CheckNIKAvailability(string nik)
        {
            var exists = await IsNIKExists(nik);
            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        public async Task<bool> IsNIKExists(string nik)
        {
            return await db.Employees.AnyAsync(e => e.NIK == nik);
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
