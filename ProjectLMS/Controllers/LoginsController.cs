using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectLMS.Models;

namespace ProjectLMS.Controllers
{
    public class LoginsController : Controller
    {
        private readonly LMS_dbContext _context;
        private readonly ILoginRepo _user;

        public LoginsController(LMS_dbContext context, ILoginRepo user)
        {
            _context = context;
            _user = user;
        }

        // GET: Logins
        public IActionResult Index(string Username, string Password)
        {
            if (Username != null || Password != null)
            {
                HttpContext.Session.SetString("Username", Username);
                var user = _user.getUserByName(Username);
                if (user == null)
                {
                    ViewBag.Message = "Invalid";


                }

                if (Username.Equals("admin") && Password.Equals("admin"))
                {
                    HttpContext.Session.SetString("Username", Username);
                    ViewBag.Message = "Success";
                    return RedirectToAction("admin_dash", "LendRequests");

                }
                else if (Username.Equals(user.Username) && Password.Equals(user.Password))
                {
                    HttpContext.Session.SetString("Username", Username);
                    ViewBag.Message = $"Login Successful for {user.Username}";
                    return RedirectToAction("User_dash", "BooksUser");

                }
                else
                {

                    ViewBag.Message = "Invalid credintials";

                }


            }
            return View();
        }
        public ActionResult admin_dash()
        {
            return View();
        }
        public ActionResult User_dash()
        {
            return View();
        }

        // GET: Logins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Logins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,Password,Role")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Logins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,Password,Role")] Account account)
        {
            if (id != account.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Logins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.UserId == id);
        }
    }
}
