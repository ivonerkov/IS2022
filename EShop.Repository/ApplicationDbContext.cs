
using EShop.Domain.Domain_models;
using EShop.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<ShopApplicationUser>
    {
        public virtual DbSet<Ticket> Tickets{ get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<TicketsInShoppingCart> TicketsInShoppingCart { get; set; }
        public virtual DbSet<ShopApplicationUser> ShopApplicationUsers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<TicketInOrder> TicketInOrders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TicketsInShoppingCart>().HasKey(c => new { c.CartId, c.TicketId });
            builder.Entity<TicketInOrder>().HasKey(c => new { c.OrderId, c.TicketId });
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
