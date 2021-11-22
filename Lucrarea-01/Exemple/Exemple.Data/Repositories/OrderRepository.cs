using Exemple.Domain.Models;
using Exemple.Domain.Repositories;
using LanguageExt;
using Example.Data.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using static Exemple.Domain.Models.Carucior;
using static LanguageExt.Prelude;
using Exemple.Data.Models;

namespace Exemple.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext dbContext;

        public OrderRepository(OrderContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<PaidCarucior>> TryGetExistingProduct() => async () => (await (
                          from g in dbContext.OrderHeader
                          join s in dbContext.Products on g.ProductIdId equals s.ProductId
                          select new { s.Code, g.OrderId, g.Total,g.Address })
                          .AsNoTracking()
                          .ToListAsync())
                          .Select(result => new PaidCarucior( result.OrderId )
                          {
                                  
                          })
                          .ToList();

        public TryAsync<Unit> TrySaveGrades(PaidCarucior total) => async () =>
        {
            var products = (await dbContext.Products.ToListAsync()).ToLookup(product => product.ProductId);
            var newGrades = total.ProductList
                                    .Select(g => new Product()
                                    {

                                    });
            var updatedGrades = total.ProductList
                                    .Select(g => new Order()
                                    {
                                       
                                    });

            dbContext.AddRange(newGrades);
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

        TryAsync<List<CalculatedPrice>> IOrderRepository.TryGetExistingProduct()
        {
            throw new System.NotImplementedException();
        }
    }
}
