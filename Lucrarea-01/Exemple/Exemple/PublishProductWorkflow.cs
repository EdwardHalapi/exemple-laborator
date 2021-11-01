using Exemple.Domain.Models;
using static Exemple.Domain.Models.PaidCaruciorEvent;
using static Exemple.Domain.PriceOperations;
using System;
using static Exemple.Domain.Models.Carucior;
using System.Threading.Tasks;
using LanguageExt;

namespace Exemple.Domain
{
    public class PublishProductWorkflow
    {
        public async Task<IPaidCarucioredEvent> ExecuteAsync(PublishQuantityCommand command, Func<ProductCode,TryAsync<bool>> checkProductExists)
        {
            UnvalidatedProduct unvalidatedGrades = new UnvalidatedProduct(command.inputQuantity);
            ICarucior products = await ValidateProducts(checkProductExists, unvalidatedGrades);
            products = CalculatePrice(products);
            products = PaidCarucior(products);

            return products.Match(
                    whenUnvalidatedProduct: unvalidatedProduct => new PaidCaruciorFaildEvent("Unexpected unvalidated state") as IPaidCarucioredEvent,
                    whenInvalidatedCarucior: invalidatedProduct => new PaidCaruciorFaildEvent(invalidatedProduct.Reason),
                    whenValidatedCarucior: validatedProduct => new PaidCaruciorFaildEvent("Unexpected validated state"),
                    whenCalculatePrice: calculatedProduct => new PaidCaruciorFaildEvent("Unexpected calculated state"),
                    whenPaidCarucior: paidProduct => new PaidCaruciorScucceededEvent(paidProduct.PublishedDate)
                );
        }
    }
}
