using System;

namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundException : ApplicationException
    {
        public OrderNotFoundException(string name, object key)
            : base($"Entity '{name}' with key '{key}' was not found.")
        {
        }
    }
}
