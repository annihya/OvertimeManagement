using OvertimeManagement.Helper;
using OvertimeManagement.Models;
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
        private readonly OvertimeManagementEntities _db;

        public OvertimesController()
        {
            _db = new OvertimeManagementEntities();
        }

        // GET: Overtimes
        public async Task<ActionResult> Index()
        {
            return View(await _db.Overtimes.ToListAsync());
        }

        // GET: Overtimes/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Overtime overtime = await _db.Overtimes.FindAsync(id);
            if (overtime == null)
            {
                return HttpNotFound();
            }
            return View(overtime);
        }

        // GET: Overtimes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Overtimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OvertimeID,EmployeeID,TimeStart,TimeFinish,ActualHours,CalculatedHours")] Overtime overtime)
        {
            if (ModelState.IsValid)
            {
                overtime.OvertimeID = Guid.NewGuid();
                _db.Overtimes.Add(overtime);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(overtime);
        }

        // GET: Overtimes/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Overtime overtime = await _db.Overtimes.FindAsync(id);
            if (overtime == null)
            {
                return HttpNotFound();
            }
            return View(overtime);
        }

        // POST: Overtimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OvertimeID,EmployeeID,TimeStart,TimeFinish,ActualHours,CalculatedHours")] Overtime overtime)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(overtime).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(overtime);
        }

        // GET: Overtimes/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Overtime overtime = await _db.Overtimes.FindAsync(id);
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
            Overtime overtime = await _db.Overtimes.FindAsync(id);
            _db.Overtimes.Remove(overtime);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
