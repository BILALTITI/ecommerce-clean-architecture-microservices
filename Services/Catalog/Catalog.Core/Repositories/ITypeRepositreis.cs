using Catalog.Core.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Core.Repositories
{
    public interface ITypeRepositreis
    {
        Task<IEnumerable<productType>> GetAllTypesAsync();
    }
}
