using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Comand
{
  public class DeleteProductCommand :IRequest<bool>
    {

        public string Id { get; set; }
        public DeleteProductCommand( string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null or empty.");
            }
            Id = id;
        }
    }
}
