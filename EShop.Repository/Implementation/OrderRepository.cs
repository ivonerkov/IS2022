using EShop.Domain.Domain_models;
using EShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }
        public List<Order> getAllOrders()
        {
            return entities.Include(z => z.OrderBy).
                Include(z => z.Tickets).
                Include("Tickets.Ticket").ToList();
        }

        public Order getOrderDetails(BaseEntity model)
        {
            return entities.Include(z => z.OrderBy).
                Include(z => z.Tickets).
                Include("Tickets.Ticket").SingleOrDefault(z => z.Id == model.Id);
        }
    }
}
