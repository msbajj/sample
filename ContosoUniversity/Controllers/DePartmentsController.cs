using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class DePartmentsController : Controller
    {
        private readonly SchoolContext _context;

        public DePartmentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: DePartments
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.DePartments.Include(d => d.Administrator);
            return View(await schoolContext.ToListAsync());
        }

        // GET: DePartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dePartment = await _context.DePartments
                .Include(d => d.Administrator)
                .SingleOrDefaultAsync(m => m.DepartmentID == id);
            if (dePartment == null)
            {
                return NotFound();
            }

            return View(dePartment);
        }

        // GET: DePartments/Create
        public IActionResult Create()
        {
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "name");
            return View();
        }

        // POST: DePartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,Name,Budget,StartDate,InstructorID,RowVersion")] DePartment dePartment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dePartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "name", dePartment.InstructorID);
            return View(dePartment);
        }

        // GET: DePartments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dePartment = await _context.DePartments
                .Include(i => i.Administrator)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.DepartmentID == id);
            if (dePartment == null)
            {
                return NotFound();
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "name", dePartment.InstructorID);
            return View(dePartment);
        }

        // POST: DePartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentToUpdate = await _context.DePartments
                .Include(i => i.Administrator)
                .SingleOrDefaultAsync(m => m.DepartmentID == id);
            if (departmentToUpdate == null)
            {
                DePartment DeletedePartment = new DePartment();
                await TryUpdateModelAsync(DeletedePartment);
                ModelState.AddModelError(string.Empty, "不能保存，被其他用户删除。");
                ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "name", DeletedePartment.InstructorID);
                return View(DeletedePartment);
            }

            _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = rowVersion;

            if (await TryUpdateModelAsync<DePartment>(departmentToUpdate, "", s => s.Name, s => s.Budget, s => s.StartDate, s => s.DepartmentID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (DePartment)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "不能保存，被其他用户删除。");
                    }
                    else
                    {
                        var databaseValues = (DePartment)databaseEntry.ToObject();
                        if (databaseValues.Name != clientValues.Name)
                        {
                            ModelState.AddModelError("Name", $"当前值: {databaseValues.Name}");
                        }
                        if (databaseValues.Budget != clientValues.Budget)
                        {
                            ModelState.AddModelError("Budget", $"当前值: {databaseValues.Budget:c}");
                        }
                        if (databaseValues.StartDate != clientValues.StartDate)
                        {
                            ModelState.AddModelError("StartDate", $"当前值: {databaseValues.StartDate:d}");
                        }
                        if (databaseValues.InstructorID != clientValues.InstructorID)
                        {
                            Instructor databaseInstructor = await _context.Instructors.SingleOrDefaultAsync(i => i.ID == databaseValues.InstructorID);
                            ModelState.AddModelError("InstructorID", $"当前值: {databaseInstructor?.name}");
                        }
                        ModelState.AddModelError(string.Empty, "你试图修改的数据，被其他人做了修改。如必须修改请再次点击保存");
                        departmentToUpdate.RowVersion = (byte[])databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", departmentToUpdate.InstructorID);
            return View(departmentToUpdate);
        }

        // GET: DePartments/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dePartment = await _context.DePartments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.DepartmentID == id);
            if (dePartment == null)
            {
                if (concurrencyError == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "数据被其他用户同一时间修改，无法删除，如果需要请重新点击删除。";
            }

            return View(dePartment);
        }

        // POST: DePartments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DePartment dePartment)
        {
            try
            {
                if (await _context.DePartments.AnyAsync(m => m.DepartmentID == dePartment.DepartmentID))
                {
                    _context.DePartments.Remove(dePartment);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
           catch(DbUpdateConcurrencyException)
            {
                return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = dePartment.DepartmentID });
            }

            
        }

        private bool DePartmentExists(int id)
        {
            return _context.DePartments.Any(e => e.DepartmentID == id);
        }
    }
}
