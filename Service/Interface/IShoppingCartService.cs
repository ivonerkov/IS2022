using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteTicketFromShoppingCart(string userId, int ticketId);
        bool orderNow(string userId);
    }
}
