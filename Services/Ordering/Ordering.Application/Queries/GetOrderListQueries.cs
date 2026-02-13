using MediatR;
using Ordering.Application.Responses;
using System.Collections.Generic;

namespace Ordering.Application.Queries
{
    public class GetOrderListQueries : IRequest<List<OrderResponse>>
    {

        public string UserName { get; set; }

        public GetOrderListQueries(string userName)
        {
            UserName = userName;
        }


        // any filters or properties if needed
    }
}
