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
    public class LendRequestsController : Controller
    {
        private readonly LMS_dbContext _context;
        private readonly ILoginRepo _login;

        public LendRequestsController(LMS_dbContext context, ILoginRepo loginRepo)
        {
            _context = context;
            _login = loginRepo;
        }

        // GET: LendRequests
        public async Task<IActionResult> Index()
        {
            var lMS_dbContext = _context.LendRequests.Include(l => l.Book).Include(l => l.User);
            return View(await lMS_dbContext.ToListAsync());
        }
        public ViewResult admin_dash()
        {
            var allreq = _context.LendRequests.Include(l => l.Book).Include(l => l.User);

            return View(allreq.ToList());
        }
        public ViewResult Lend(int bookId)
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _login.getUserByName(username);
            var noofcopies = _context.Books.SingleOrDefault(b => b.BookId == bookId).NoOfCopies;


            if (noofcopies <= 0)
            {
                return View("RequestedError");
            }
            _context.Books.SingleOrDefault(b => b.BookId == bookId).NoOfCopies--;
            LendRequest lendRequest = new LendRequest()
            {
                LendStatus = "Requested",
                LendDate = DateTime.Now,
                BookId = bookId,
                UserId = user.UserId,
                Book = _context.Books.SingleOrDefault(b => b.BookId == bookId),
                User = _context.Accounts.SingleOrDefault(u => u.UserId == user.UserId),
            };
            _context.LendRequests.Add(lendRequest);
            _context.SaveChanges();

            return View();
        }
        public ViewResult Approved(int bookId)
        {


            return View();
        }
        public ViewResult Declined(int bookId)
        {

            return View();
        }

        // GET: LendRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lendRequest = await _context.LendRequests
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LendId == id);
            if (lendRequest == null)
            {
                return NotFound();
            }

            return View(lendRequest);
        }

        // GET: LendRequests/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookTitle");
            ViewData["UserId"] = new SelectList(_context.Accounts, "UserId", "Password");
            return View();
        }

        // POST: LendRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LendId,LendStatus,LendDate,ReturnDate,UserId,BookId,FineAmount")] LendRequest lendRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lendRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookTitle", lendRequest.BookId);
            ViewData["UserId"] = new SelectList(_context.Accounts, "UserId", "Password", lendRequest.UserId);
            return View(lendRequest);
        }

        // GET: LendRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lendRequest = await _context.LendRequests.FindAsync(id);
            if (lendRequest == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookTitle", lendRequest.BookId);
            ViewData["UserId"] = new SelectList(_context.Accounts, "UserId", "Password", lendRequest.UserId);
            return View(lendRequest);
        }

        // POST: LendRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LendId,LendStatus,LendDate,ReturnDate,UserId,BookId,FineAmount")] LendRequest lendRequest)
        {
            if (id != lendRequest.LendId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lendRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LendRequestExists(lendRequest.LendId))
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
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookTitle", lendRequest.BookId);
            ViewData["UserId"] = new SelectList(_context.Accounts, "UserId", "Password", lendRequest.UserId);
            return View(lendRequest);
        }

        // GET: LendRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lendRequest = await _context.LendRequests
                .Include(l => l.Book)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.LendId == id);
            if (lendRequest == null)
            {
                return NotFound();
            }

            return View(lendRequest);
        }

        // POST: LendRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lendRequest = await _context.LendRequests.FindAsync(id);
            _context.LendRequests.Remove(lendRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LendRequestExists(int id)
        {
            return _context.LendRequests.Any(e => e.LendId == id);
        }
    }
}
