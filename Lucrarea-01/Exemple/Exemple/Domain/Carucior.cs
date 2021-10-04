using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    [AsChoice]
    public static partial class Carucior
    {
        public interface ICarucior { }

        public record EmptyCarucior(IReadOnlyCollection<UnvalidatedProduct> ProductList) : ICarucior;

        public record InvalidatedCarucior(IReadOnlyCollection<UnvalidatedProduct> ProductList, string reason) : ICarucior;

        public record ValidatedCarucior(IReadOnlyCollection<ValidatedProduct> ProductList) : ICarucior;

        public record PaidCarucior(IReadOnlyCollection<ValidatedProduct> ProductList, DateTime PublishedDate) : ICarucior;
    }
}
