using Exemple.Domain.Models;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.Models.Carucior;
using static LanguageExt.Prelude;

namespace Exemple.Domain
{
    public static class PriceOperations
    {
        public static Task<ICarucior> ValidateProducts(Func<ProductCode, TryAsync<bool>> checkProductExists, UnvalidatedProduct product)=>

            product.ProductList
                      .Select(ValidateProduct(checkProductExists))
                      .Aggregate(CreateEmptyValatedQuantitysList().ToAsync(), ReduceValidQuantitys)
                      .MatchAsync(
                            Right: validatedProducts => new ValidatedCarucior(validatedProducts),
                            LeftAsync: errorMessage => Task.FromResult((ICarucior)new InvalidatedCarucior(product.ProductList, errorMessage))
                      );

        private static Func<UnvalidatedProductQuantity, EitherAsync<string, ValidatedProduct>> ValidateProduct(Func<ProductCode, TryAsync<bool>> checkProductExists) =>
                unValidatedProduct => ValidateProduct(checkProductExists, unValidatedProduct);

            private static EitherAsync<string, ValidatedProduct> ValidateProduct(Func<ProductCode, TryAsync<bool>> checkProductExists, UnvalidatedProductQuantity product) =>
                from quantity in Quantity.TryParseQuantity(product.quantity)
                                       .ToEitherAsync(() => $"Invalid  Quantity ({product.cod}, {product.quantity})")
                from productCod in ProductCode.TryParse(product.cod)
                                       .ToEitherAsync(() => $"Invalid product code ({product.cod})")
                from clientAddress in Adresa.TryParse(product.address)
                                       .ToEitherAsync(() => $"Invalid client address ({product.address})")
                from prodExists in checkProductExists(productCod)
                                       .ToEither(error => error.ToString())
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

            //if (isValidList)
            //{
            //    return new ValidatedCarucior(validatedProduct);
            //}
            //else
            //{
            //    return new InvalidatedCarucior(product.ProductList, invalidReson);
            //}

        

        public static ICarucior CalculatePrice(ICarucior products) => products.Match(
            whenUnvalidatedProduct: unvalidaTedProduct => unvalidaTedProduct,
            whenInvalidatedCarucior: invalidProduct => invalidProduct,
            whenCalculatePrice: calculatedPrice => calculatedPrice,
            whenPaidCarucior: paidCarucior => paidCarucior,
            whenValidatedCarucior: validProduct =>
            {
                var calculatedPrice = validProduct.ProductList.Select(validProduct =>
                                                  new CalculatedPrice(validProduct.ProductCode,
                                                                      validProduct.Quantity,
                                                                      validProduct.Quantity));
                return new CalculatePrice(calculatedPrice.ToList().AsReadOnly());
            }
        );

        public static ICarucior PaidCarucior(ICarucior products) => products.Match(
            whenUnvalidatedProduct: unvalidaTedProduct => unvalidaTedProduct,
            whenInvalidatedCarucior: invalidProduct => invalidProduct,
            whenCalculatePrice: calculatedPrice => calculatedPrice,
            whenPaidCarucior: paidCarucior => paidCarucior,
            whenValidatedCarucior: validProduct =>
            {
                PaidCarucior publishedproduct = new(validProduct.ProductList, DateTime.Now);

                return publishedproduct;
            });
    }
}
