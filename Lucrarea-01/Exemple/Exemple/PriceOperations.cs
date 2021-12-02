using Exemple.Domain.Models;
using LanguageExt;
using static LanguageExt.Prelude;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.Models.Carucior;


namespace Exemple.Domain
{
    public static class PriceOperations
    {
        public static Task<ICarucior> ValidateProducts(Func<ProductCode, Option<ProductCode>> checkProductExists, UnvalidatedProduct product)=>

            product.ProductList
                      .Select(ValidateProduct(checkProductExists))
                      .Aggregate(CreateEmptyValatedQuantitysList().ToAsync(), ReduceValidQuantitys)
                      .MatchAsync(
                            Right: validatedProducts => new ValidatedCarucior(validatedProducts),
                            LeftAsync: errorMessage => Task.FromResult((ICarucior)new InvalidatedCarucior(product.ProductList, errorMessage))
                      );

        private static Func<UnvalidatedProductQuantity, EitherAsync<string, ValidatedProduct>> ValidateProduct(Func<ProductCode, Option<ProductCode>> checkProductExists) =>
                unValidatedProduct => ValidateProduct(checkProductExists, unValidatedProduct);

            private static EitherAsync<string, ValidatedProduct> ValidateProduct(Func<ProductCode, Option<ProductCode>> checkProductExists, UnvalidatedProductQuantity product) =>
                from quantity in Quantity.TryParseQuantity(product.quantity)
                                       .ToEitherAsync(() => $"Invalid  Quantity ({product.cod}, {product.quantity})")
                from productCod in ProductCode.TryParse(product.cod)
                                       .ToEitherAsync(() => $"Invalid product code ({product.cod})")
                from clientAddress in Adresa.TryParse(product.address)
                                       .ToEitherAsync(() => $"Invalid client address ({product.address})")
                from prodExists in checkProductExists(productCod)
                                       .ToEitherAsync($"Student {productCod.Value} does not exist.")
                select new ValidatedProduct(productCod, quantity, clientAddress);

            private static Either<string, List<ValidatedProduct>> CreateEmptyValatedQuantitysList() =>
                Right(new List<ValidatedProduct>());

            private static EitherAsync<string, List<ValidatedProduct>> ReduceValidQuantitys(EitherAsync<string, List<ValidatedProduct>> acc, EitherAsync<string, ValidatedProduct> next) =>
                from list in acc
                from nextQuantity in next
                select list.AppendValidQuantity(nextQuantity);

            private static List<ValidatedProduct> AppendValidQuantity(this List<ValidatedProduct> list, ValidatedProduct validQuantity)
            {
                list.Add(validQuantity);
                return list;
            }

        public static ICarucior CalculateFinalFinalQuantitys(ICarucior products) => products.Match(
           whenUnvalidatedProduct: unvalidatedProduct => unvalidatedProduct,
           whenInvalidatedCarucior: invalidatedCarucior => invalidatedCarucior,
           whenFailedCalculatedPrice: failCalculatePrice => failCalculatePrice,
           whenCalculatedPrice: calculatedPrice => calculatedPrice,
           whenPaidCarucior: paidCarucior => paidCarucior,
           whenValidatedCarucior: CalculateFinalQuantity
       );
       private static ICarucior CalculateFinalQuantity(ValidatedCarucior product)=>
         new CalculatedPrice(product.ProductList
                                    .Select(CalculatedCustomerFinalPrice)
                                    .ToList()
                                    .AsReadOnly());
        private static CalculatedCustomerPrice CalculatedCustomerFinalPrice(ValidatedProduct prod) =>
            new CalculatedCustomerPrice(prod.ProductCode,
                                        prod.Quantity,
                                        prod.Quantity);

        public static ICarucior MergePrices(ICarucior products, IEnumerable<CalculatedCustomerPrice> existingPrices) => products.Match(
            whenUnvalidatedProduct: unvalidatedProduct => unvalidatedProduct,
            whenInvalidatedCarucior: invalidatedCarucior => invalidatedCarucior,
            whenFailedCalculatedPrice: failCalculatePrice => failCalculatePrice,
            whenPaidCarucior: paidCarucior => paidCarucior,
            whenValidatedCarucior: validatedCarucior => validatedCarucior,
            whenCalculatedPrice: calculatedPrice => MergePrices(calculatedPrice.ProductList, existingPrices));

        private static CalculatedPrice MergePrices(IEnumerable<CalculatedCustomerPrice> newList, IEnumerable<CalculatedCustomerPrice> existingList)
        {
           var updatedAndNewPrices = newList.Select(price => price with { ProductId = existingList.FirstOrDefault(p => p.ProductCode == price.ProductCode)?.ProductId ?? 0, IsUpdated = true });
            var oldPrices = existingList.Where(price => !newList.Any(p => p.ProductCode == price.ProductCode));
            var allPrices = updatedAndNewPrices.Union(oldPrices)
                .ToList()
                .AsReadOnly();
                return new CalculatedPrice(allPrices);
        }
    }
}
