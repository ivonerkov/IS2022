using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Service.Interface;
using EShop.Domain.DTO;
using EShop.Domain.Domain_models;

namespace IS2022.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            return View(_ticketService.GetAllTickets());
        }
        public async Task<IActionResult> AddToCart(int ticketId)
        {
            var ticket = _ticketService.GetDetailsForTicket(ticketId);
            var model = new AddToShoppingCartDto();
            model.SelectedTicket = ticket;
            model.TicketId = ticket.Id;
            model.Quantity = 0;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToShoppingCart(AddToShoppingCartDto model)
        {
            /*var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.Where(z => z.Id == userId).Include("UserShoppingCart.TicketsInShoppingCart").
                Include("UserShoppingCart.TicketsInShoppingCart.Ticket")
                .FirstOrDefault();

            var userShoppingCart = user.UserShoppingCart;
            if (userShoppingCart != null)
            {
                var ticket = _context.Tickets.Find(model.TicketId);
                if (ticket != null)
                {
                    TicketsInShoppingCart itemToAdd = new TicketsInShoppingCart
                    {
                        Ticket = ticket,
                        TicketId = ticket.TicketId,
                        ShoppingCart = userShoppingCart,
                        Quantity = model.Quantity
                    };
                    _context.Add(itemToAdd);
                    await _context.SaveChangesAsync();
                }
            }*/
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._ticketService.AddToShoppingCart(model, userId);

            if (result)
            {
                return RedirectToAction("Index", "Tickets");
            }
            return RedirectToAction("Index");
        }











        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetDetailsForTicket(id??0);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _ticketService.CreateNewTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetDetailsForTicket(id ?? 0);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ticketService.UpdateExistingProducst(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetDetailsForTicket(id ?? 0);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _ticketService.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _ticketService.GetDetailsForTicket(id) != null;
        }
    }
}
