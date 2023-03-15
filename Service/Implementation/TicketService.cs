
using EShop.Domain.Domain_models;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Implementation
{
    public class TicketService : ITicketService
    {
        public readonly IRepository<Ticket> _ticketRepository;
        public readonly IRepository<TicketsInShoppingCart> _ticketInShoppingCartRepository;
        public readonly IUserRepository _userRepository;

        public TicketService(IRepository<TicketsInShoppingCart> ticketInShoppingCartRepository, IRepository<Ticket> tikcetRepository, IUserRepository userRepository)
        {
            _ticketRepository = tikcetRepository;
            _userRepository = userRepository;
            _ticketInShoppingCartRepository = ticketInShoppingCartRepository;

        }
        public bool AddToShoppingCart(AddToShoppingCartDto item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userShoppingCard = user.UserShoppingCart;

            if (userShoppingCard != null)
            {
                var ticket = this.GetDetailsForTicket(item.TicketId);

                if (ticket != null)
                {
                    TicketsInShoppingCart itemToAdd = new TicketsInShoppingCart
                    {
                        Ticket = ticket,
                        TicketId = ticket.Id,
                        ShoppingCart = userShoppingCard,
                        CartId = userShoppingCard.Id,
                        Quantity = item.Quantity
                    };
                    _ticketInShoppingCartRepository.Insert(itemToAdd);
                    return true;
                }
                return false;
            }
            return false;
        }

        public void CreateNewTicket(Ticket p)
        {
            this._ticketRepository.Insert(p);
        }

        public void DeleteTicket(int id)
        {
            var ticket = _ticketRepository.Get(id);
            this._ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            return _ticketRepository.GetAll().ToList();
        }

        public Ticket GetDetailsForTicket(int id)
        {
            return _ticketRepository.Get(id);
        }

        public ShoppingCartDto GetShoppingCartInfo(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateExistingProducst(Ticket p)
        {
            _ticketRepository.Update(p);
        }
    }
}
