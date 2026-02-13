using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Commands
{
    public class DeleteOrderCommand :IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
