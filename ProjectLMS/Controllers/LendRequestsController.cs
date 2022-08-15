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
            var username = HttpContext.Session.GetString("Username");
            var user = _login.getUserByName(username);
            var User = _context.LendRequests.Where(u => u.UserId == user.UserId).Include(i => i.Book).Include(i => i.User);

            return View(await User.ToListAsync());

            
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
            _context.Books.SingleOrDefault(b => b.BookId == bookId).IssuedBooks++;
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
        public async Task<IActionResult> All()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _login.getUserByName(username);
            var User = _context.LendRequests.Where(u => u.UserId == user.UserId && u.LendStatus.Equals("Approved")).Include(i=>i.Book).Include(i=>i.User);
            
            return View(await User.ToListAsync());

           
        }
        public ActionResult Approved(int LendId)
        {
            var username = HttpContext.Session.GetString("UserName");
            var user = _login.getUserByName(username);
            var allreq = _context.Books.Include(l => l.Author).Include(l => l.Publisher);

            var lr = _context.LendRequests.FirstOrDefault(l => l.LendId == LendId);
           

            lr.LendStatus = "Approved";
            _context.SaveChanges();



            return RedirectToAction("admin_dash","Lendrequests");

        }
        public ActionResult Return(int LendId, int bookId)
        {
            var username = HttpContext.Session.GetString("UserName");
            var user = _login.getUserByName(username);
            var allreq = _context.Books.Include(l => l.Author).Include(l => l.Publisher);

            var lr = _context.LendRequests.FirstOrDefault(l => l.LendId == LendId);
            var expected_date = lr.LendDate.AddDays(3);
            lr.ReturnDate = DateTime.Now;
            lr.FineAmount = 0;


            if (lr.ReturnDate > expected_date)
            {
                var n = lr.ReturnDate.Subtract(expected_date).TotalDays;
                lr.FineAmount = lr.FineAmount + (n * 10);
            }
            _context.Books.SingleOrDefault(b => b.BookId == bookId).NoOfCopies++;
            _context.Books.SingleOrDefault(b => b.BookId == bookId).IssuedBooks--;
            lr.LendStatus = "Returned";
            _context.SaveChanges();



            return RedirectToAction("All", "Lendrequests");

        }
        public ActionResult Declined(int LendId)
        {
            var lr = _context.LendRequests.FirstOrDefault(l => l.LendId == LendId);

            lr.LendStatus = "Declined";
            _context.SaveChanges();

            return RedirectToAction("admin_dash", "Lendrequests"); ;
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
