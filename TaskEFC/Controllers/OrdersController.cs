using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskEFC.Data;
using TaskEFC.Models;

namespace TaskEFC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ShoppingContext _context;

        public OrdersController(ShoppingContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int? id)
        {
            var viewModel = new OrderIndexData
            {
                Orders = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.SuperMarket)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .AsNoTracking()
                    .OrderBy(o => o.OrderDate)
                    .ToListAsync()
            };

            if (id != null)
            {
                ViewData["OrderId"] = id.Value;
                var order = viewModel.Orders.SingleOrDefault(o => o.Id == id.Value);
                if (order != null)
                {
                    viewModel.OrderDetails = order.OrderDetails;
                }
            }

            return View(viewModel);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.SuperMarket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderDate,CustomerId,SuperMarketId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(order.CustomerId, order.SuperMarketId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            PopulateDropdowns(order.CustomerId, order.SuperMarketId);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,CustomerId,SuperMarketId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            PopulateDropdowns(order.CustomerId, order.SuperMarketId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.SuperMarket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns(object? selectedCustomer = null, object? selectedSuperMarket = null)
        {
            var customers = _context.Customers
                .Select(c => new { c.Id, FullName = c.FirstName + " " + c.LastName })
                .ToList();

            ViewData["CustomerId"] = new SelectList(customers, "Id", "FullName", selectedCustomer);
            ViewData["SuperMarketId"] = new SelectList(_context.SuperMarkets, "Id", "Name", selectedSuperMarket);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}