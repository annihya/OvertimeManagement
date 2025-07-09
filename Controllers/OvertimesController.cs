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
using System.Threading;

namespace OvertimeManagement.Controllers
{
    public class OvertimesController : Controller
    {
        private DBModel db = new DBModel();

        // GET: Overtimes
        public async Task<ActionResult> Index(CancellationToken cancellationToken = default)
        {
            return View(await db.Overtimes.ToListAsync(cancellationToken));
        }

        // GET: Overtimes/Details/5
        public async Task<ActionResult> Details(Guid? id)
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
        public async Task<ActionResult> Create([Bind(Include = "OvertimeID,EmployeeID,TimeStart,TimeFinish,ActualHours,CalculatedHours")] Overtime overtime, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                overtime.OvertimeID = Guid.NewGuid();
                db.Overtimes.Add(overtime);
                await db.SaveChangesAsync(cancellationToken);
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
            Overtime overtime = await db.Overtimes.FindAsync(id);
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
        public async Task<ActionResult> Edit([Bind(Include = "OvertimeID,EmployeeID,TimeStart,TimeFinish,ActualHours,CalculatedHours")] Overtime overtime, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                db.Entry(overtime).State = EntityState.Modified;
                await db.SaveChangesAsync(cancellationToken);
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
        public async Task<ActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken = default)
        {
            Overtime overtime = await db.Overtimes.FindAsync(id);
            db.Overtimes.Remove(overtime);
            await db.SaveChangesAsync(cancellationToken);
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
