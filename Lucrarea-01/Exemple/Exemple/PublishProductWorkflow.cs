using Exemple.Domain.Models;
using static Exemple.Domain.Models.PaidCaruciorEvent;
using static Exemple.Domain.PriceOperations;
using System;
using static Exemple.Domain.Models.Carucior;
using System.Threading.Tasks;
using LanguageExt;
using Exemple.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using static LanguageExt.Prelude;

namespace Exemple.Domain
{
    public class PublishProductWorkflow
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly ILogger<PublishProductWorkflow> logger;
        public PublishProductWorkflow(IOrderRepository orderRepository, IProductRepository productRepository, ILogger<PublishProductWorkflow> logger)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.logger = logger;
        }

        public async Task<IPaidCarucioredEvent> ExecuteAsync(PublishQuantityCommand command)
        {
            UnvalidatedProduct unvalidatedProducts = new UnvalidatedProduct(command.inputQuantity);

            var result = from products in productRepository.TryGetExistingProduct(unvalidatedProducts.ProductList.Select(prod => prod.cod))
                                           .ToEither(ex => new FailedCalculatedPrice(unvalidatedProducts.ProductList, ex) as ICarucior)
                         from existingProd in orderRepository.TryGetExistingProduct()
                                          .ToEither(ex => new FailedCalculatedPrice(unvalidatedProducts.ProductList, ex) as ICarucior)
                         let checkProdExists = (Func<ProductCode, Option<ProductCode>>)(prod => CheckProdExists(products, prod))
                         from publishedProduct in ExecuteWorkflowAsync(unvalidatedProducts, existingProd, checkProdExists).ToAsync()
                         from _ in orderRepository.TrySaveproduct(publishedProduct)
                                          .ToEither(ex => new FailedCalculatedPrice(unvalidatedProducts.ProductList, ex) as ICarucior)
                         select publishedProduct;

            return await result.Match(
                    Left: products => GenerateFailedEvent(products) as IPaidCarucioredEvent,
                    Right: publishedProds => new PaidCarucior(publishedProds.ProductList, publishedProds.PublishedDate)
                );
        }
        private async Task<Either<ICarucior, PaidCarucior>> ExecuteWorkflowAsync(UnvalidatedProduct unvalidatedProducts,
                                                                                          IEnumerable<CalculatedCustomerPrice> existingprods,
                                                                                          Func<ProductCode, Option<ProductCode>> checkProdExists)
        {

            ICarucior products = await ValidateProducts(checkProdExists, unvalidatedProducts);
            products = CalculateFinalFinalQuantitys(products);
            products = MergePrices(products, existingprods);

            return products.Match<Either<ICarucior, PaidCarucior>>(
                whenUnvalidatedProduct: unvalidatedProducts => Left(unvalidatedProducts as ICarucior),
                whenInvalidatedCarucior: invalidatedCarucior => Left(invalidatedCarucior as ICarucior),
                whenValidatedCarucior: ValidatedCarucior => Left(ValidatedCarucior as ICarucior),
                whenCalculatedPrice: calculatedPrice => Left(calculatedPrice as ICarucior),
                whenFailedCalculatedPrice: failedCalculatedPrice => Left(failedCalculatedPrice as ICarucior),
                whenPaidCarucior: paidCarucior => Right(paidCarucior)
            );
        }

        private Option<ProductCode> CheckProdExists(IEnumerable<ProductCode> products, ProductCode prodCode)
        {
            if (products.Any(s => s == prodCode))
            {
                return Some(prodCode);
            }
            else
            {
                return None;
            }
        }

        private PaidCaruciorFaildEvent GenerateFailedEvent(ICarucior prods) =>
               prods.Match<PaidCaruciorFaildEvent>(
                   whenUnvalidatedProduct: unvalidatedProduct => new($"Invalid state{nameof(UnvalidatedProduct)}"),
                   whenInvalidatedCarucior: invalidProd => new(invalidProd.Reason),
                   whenValidatedCarucior: validatedProd => new($"Invalid state {nameof(ValidatedCarucior)}"),
                   whenCalculatedPrice: calculatedPrice => new($"Invalid state {nameof(CalculatedPrice)}"),
                   whenFailedCalculatedPrice: failedCalculatedPrice =>
                    {
                        logger.LogError(failedCalculatedPrice.Exception, failedCalculatedPrice.Exception.Message);
                        return new(failedCalculatedPrice.Exception.Message);
                    },
                   whenPaidCarucior: paidCarucior => new($"Invalid state {nameof(paidCarucior)}"));
    }
}
