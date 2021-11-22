using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exemple.Domain.Models;
using Exemple.Domain.Repositories;
using LanguageExt;
using Example.Data.Models;
using Microsoft.EntityFrameworkCore;
using static Exemple.Domain.Models.Carucior;
using static LanguageExt.Prelude;
using Exemple.Data.Models;

namespace Exemple.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OrderContext orderContext;

        public ProductRepository(OrderContext orderContext)
        {
            this.orderContext = orderContext;
        }

        public TryAsync<List<ProductCode>> TryGetExistingProduct(IEnumerable<string> productToCheck) => async () =>
        {
            var students = await orderContext.Products
                                              .Where(product => productToCheck.Contains(product.Code))
                                              .AsNoTracking()
                                              .ToListAsync();
            return students.Select(product => new ProductCode(product.Code))
                           .ToList();
        };
    }
}
