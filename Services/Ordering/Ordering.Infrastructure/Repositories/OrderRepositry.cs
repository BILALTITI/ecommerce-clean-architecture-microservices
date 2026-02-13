using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entites;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepositry : RepositoryBase<Order>, IOrderReposirtoy
    {
 
        public OrderRepositry(OrderContext orderContext) : base(orderContext)
        {
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
        
            var orderlist = await _orderContext.Orders.Where(o => o.UserName == userName).ToListAsync();

            return orderlist;
        }
    }
}
