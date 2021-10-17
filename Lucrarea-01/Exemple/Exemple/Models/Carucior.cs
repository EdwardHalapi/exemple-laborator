using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class Carucior
    {
        public interface ICarucior { }

        public record UnvalidatedProduct : ICarucior
        {
            public UnvalidatedProduct(IReadOnlyCollection<UnvalidatedProductQuantity> productList)
            {
                ProductList = productList;
            }
            public IReadOnlyCollection<UnvalidatedProductQuantity> ProductList { get; }
        }
        public record InvalidatedCarucior : ICarucior
        {
            internal InvalidatedCarucior(IReadOnlyCollection<UnvalidatedProductQuantity> productlist, string reason)
            {
                productlist = ProductList;
                reason = Reason;
            }
            public IReadOnlyCollection<UnvalidatedProductQuantity> ProductList { get; }
            public string Reason { get; }
        }

        public record ValidatedCarucior : ICarucior
        {
            internal ValidatedCarucior(IReadOnlyCollection<ValidatedProduct> productlist)
            {
                productlist = ProductList;
            }
            public IReadOnlyCollection<ValidatedProduct> ProductList { get; }
        }
        public record CalculatePrice : ICarucior
        {
            internal CalculatePrice(IReadOnlyCollection<CalculatedPrice> productlist)
            {
                productlist = ProductList;
            }
            public IReadOnlyCollection<CalculatedPrice> ProductList { get; }
        }
        public record PaidCarucior : ICarucior
        {
            internal PaidCarucior(IReadOnlyCollection<ValidatedProduct> productList, DateTime publishedDate)
            {
                productList = ProductList;
                publishedDate = PublishedDate;
            }
            public IReadOnlyCollection<ValidatedProduct> ProductList { get; }
            public DateTime PublishedDate { get; }
        }
    }
}
