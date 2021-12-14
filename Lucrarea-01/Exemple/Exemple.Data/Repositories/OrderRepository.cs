using Exemple.Domain.Models;
using Exemple.Domain.Repositories;
using LanguageExt;
using Exemple.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Exemple.Domain.Models.Carucior;
using static LanguageExt.Prelude;

namespace Exemple.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext dbContext;

        public OrderRepository(OrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<ProductCode>> TryGetExistingCarucior(IEnumerable<string> productToCheck) => async () =>
        {
            var products = await dbContext.Products
                                              .Where(product => productToCheck.Contains(product.Code))
                                              .AsNoTracking()
                                              .ToListAsync();
            return products.Select(product => new ProductCode(product.Code))
                           .ToList();
        };
    }
}
