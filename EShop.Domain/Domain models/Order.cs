using EShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Domain.Domain_models
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public ShopApplicationUser OrderBy { get; set; }
        public List<TicketInOrder> Tickets { get; set; }
    }
}
