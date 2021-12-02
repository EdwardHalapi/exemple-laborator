using CSharp.Choices;
using LanguageExt.Common;
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
        public record CalculatedPrice : ICarucior
        {
            internal CalculatedPrice(IReadOnlyCollection<CalculatedCustomerPrice> productlist)
            {
                productlist = ProductList;
            }
            public IReadOnlyCollection<CalculatedCustomerPrice> ProductList { get; }
        }
        public record FailedCalculatedPrice : ICarucior
        {

            internal FailedCalculatedPrice(IReadOnlyCollection<UnvalidatedProductQuantity> productList, Exception exception)
            {
                ProductList = productList;
                Exception = exception;
            }

            public IReadOnlyCollection<UnvalidatedProductQuantity> ProductList { get; }
            public Exception Exception { get; }
        }
        public record PaidCarucior : ICarucior
        {
            private int orderId;

            public PaidCarucior(int orderId)
            {
                this.orderId = orderId;
            }

            internal PaidCarucior(IReadOnlyCollection<CalculatedCustomerPrice> productList, DateTime publishedDate)
            {
                productList = ProductList;
                publishedDate = PublishedDate;
            }
            public IReadOnlyCollection<CalculatedCustomerPrice> ProductList { get; }
            public DateTime PublishedDate { get; }
        }
    }
}
