
using EShop.Domain.Domain_models;
using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Ticket GetDetailsForTicket(int id);
        void CreateNewTicket(Ticket p);
        void UpdateExistingProducst(Ticket p);
        ShoppingCartDto GetShoppingCartInfo(int id);
        void DeleteTicket(int id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
    }
}
