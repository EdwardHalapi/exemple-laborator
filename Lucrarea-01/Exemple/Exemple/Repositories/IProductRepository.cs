using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exemple.Domain.Models;
using LanguageExt;

namespace Exemple.Domain.Repositories
{
    public interface IProductRepository
    {
        TryAsync<List<ProductCode>> TryGetExistingProduct(IEnumerable<string> ProductToCheck);
    }
}
