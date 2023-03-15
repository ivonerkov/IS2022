using System.Collections.Generic;

namespace EShop.Domain.Domain_models
{
    public class ShoppingCart : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public ICollection<TicketsInShoppingCart> TicketsInShoppingCart { get; set; }
    }
}
