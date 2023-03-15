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
    public class ShoppingCartService : IShoppingCartService
    {
        public readonly IUserRepository _userRepository;
        public readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        public readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _mailrepository;
        public ShoppingCartService(IRepository<EmailMessage> mailrepository, IRepository<Order> orderRepository, IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<TicketInOrder> ticketInOrderRepository )
        {
            _userRepository = userRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _mailrepository = mailrepository;

        }
        public bool deleteTicketFromShoppingCart(string userId, int ticketId)
        {
            if(!string.IsNullOrEmpty(userId) && ticketId != null)
            {
                var loggedUser = _userRepository.Get(userId);
                var userShoppingCart = loggedUser.UserShoppingCart;
                var itemToDelete = userShoppingCart.TicketsInShoppingCart.Where(z => z.TicketId == ticketId).FirstOrDefault();
                userShoppingCart.TicketsInShoppingCart.Remove(itemToDelete);
                _shoppingCartRepository.Update(userShoppingCart);
                return true;
            }
            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var user = _userRepository.Get(userId);

            var userShoppingCart = user.UserShoppingCart;
            var ticketList = userShoppingCart.TicketsInShoppingCart.Select(z => new
            {
                Quantity = z.Quantity,
                TicketPrice = z.Ticket.TicketPrice
            });

            float totalPrice = 0;
            foreach (var item in ticketList)
            {
                totalPrice += item.TicketPrice * item.Quantity;
            }

            ShoppingCartDto model = new ShoppingCartDto
            {
                TotalPrice = totalPrice,
                TicketsInShoppingCart = userShoppingCart.TicketsInShoppingCart.ToList()

            };
            return model;
        }

        public bool orderNow(string userId)
        {
            var user = _userRepository.Get(userId);

            var userShoppingCart = user.UserShoppingCart;

            EmailMessage emailMessage = new EmailMessage();
            emailMessage.MailTo = user.Email;
            emailMessage.Subject = "Successfully created order!";
            emailMessage.Status = false;


            Order newOrder = new Order
            {
                UserId = user.Id,
                OrderBy = user
            };
            _orderRepository.Insert(newOrder);
            List<TicketInOrder> ticketInOrder = userShoppingCart.TicketsInShoppingCart.Select(z => new TicketInOrder
            {
                Ticket = z.Ticket,
                TicketId = z.TicketId,
                Order = newOrder,
                OrderId = newOrder.Id,
                Quantity = z.Quantity
            }).ToList();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Your order is completed. The order contains: ");

            var totalPrice = 0.0;
            
            for(int i = 1; i < ticketInOrder.Count(); i++)
            {

                var item = ticketInOrder[i - 1];
                totalPrice += item.Quantity * item.Ticket.TicketPrice;
                sb.AppendLine(i.ToString() + ", " + item.Ticket.MovieName + "with price of: " + item.Ticket.TicketPrice + "and quantity of:" + item.Quantity);

            }


            sb.AppendLine("Total price: " + totalPrice.ToString());
            emailMessage.Content = sb.ToString();


            foreach (var item in ticketInOrder)
            {
                _ticketInOrderRepository.Insert(item);
            }
            user.UserShoppingCart.TicketsInShoppingCart.Clear();

            this._mailrepository.Insert(emailMessage);

            _userRepository.Update(user);
            return true;
        }
    }
}
