using System.Collections.Generic;
using System.Linq;
using Exemple.Domain.Models;
using Exemple.Domain.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

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
            var products = await orderContext.Products
                                              .Where(product => productToCheck.Contains(product.Code))
                                              .AsNoTracking()
                                              .ToListAsync();
            return products.Select(product => new ProductCode(product.Code))
                           .ToList();
        };
    }
}
