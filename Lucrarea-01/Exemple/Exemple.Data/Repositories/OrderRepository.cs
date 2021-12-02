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

        public TryAsync<List<CalculatedCustomerPrice>> TryGetExistingProduct() => async () => (await (
                          from g in dbContext.OrderHeader
                          join s in dbContext.Products on g.ProductIdId equals s.ProductId
                          select new { s.Code, g.OrderId, g.Total, g.Address })
                          .AsNoTracking()
                          .ToListAsync())
                          .Select(result => new CalculatedCustomerPrice(ProductCode: new(result.Code),
                                                                            Quantity: new(result.Total ?? 0m),
                                                                            price: new(result.Total ?? 0m))
                          {
                              ProductId = result.OrderId
                          }).ToList();

        public TryAsync<Unit> TrySaveProducts(PaidCarucior total) => async () =>
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
            var updatedGrades = total.ProductList
                                    .Where(p => p.IsUpdated && p.ProductId >0)
                                    .Select(p => new Product()
                                    {
                                        ProductId = products[p.ProductCode.Value].Single().ProductId,
                                        Code = p.ProductCode.Value,
                                        Stoc = p.Quantity.Value,
                                    });

            dbContext.AddRange(newProducts);
            foreach (var entity in updatedGrades)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

            await dbContext.SaveChangesAsync();

            return unit;
        };

        public TryAsync<Unit> TrySaveproduct(PaidCarucior total)
        {
            throw new System.NotImplementedException();
        }

        TryAsync<List<CalculatedCustomerPrice>> IOrderRepository.TryGetExistingProduct()
        {
            throw new System.NotImplementedException();
        }
    }
}
