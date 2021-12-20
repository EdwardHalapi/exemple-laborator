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
    public class OrderLineRepository : IOrderLineRepository
    {
         private readonly OrderContext dbContext;

        public OrderLineRepository(OrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<CalculatedCustomerPrice>> TryGetExistingProduct() => async () => (await (
                          from g in dbContext.OrderLine
                          join s in dbContext.Products on g.OrderLineId equals s.ProductId
                          select new { s.Code, g.OrderLineId, g.Qunatity,g.Price})
                          .AsNoTracking()
                          .ToListAsync())
                          .Select(result => new CalculatedCustomerPrice(ProductCode: new(result.Code),
                                                                            Quantity: new(result.Qunatity),
                                                                            price: new(result.Price))
                          {
                              ProductId = result.OrderLineId
                          }).ToList();

        public TryAsync<Unit> TrySaveproduct(PaidCarucior total) => async () =>
        {
            var products = (await dbContext.Products.ToListAsync()).ToLookup(product => product.Code);
            var newProducts = total.ProductList
                                    .Where(p => p.IsUpdated && p.ProductId == 0)
                                    .Select(p => new Product()
                                    {
                                        ProductId = products[p.ProductCode.Value].Single().ProductId,
                                        Code = p.ProductCode.Value,
                                        Stoc = p.Quantity.Value,
                                    });
            var updatedProducts = total.ProductList
                                    .Where(p => p.IsUpdated && p.ProductId >0)
                                    .Select(p => new Product()
                                    {
                                        ProductId = products[p.ProductCode.Value].Single().ProductId,
                                        Code = p.ProductCode.Value,
                                        Stoc = p.Quantity.Value,
                                    });

            dbContext.AddRange(newProducts);
            foreach (var entity in updatedProducts)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();

            return unit;
        };
    }
}
