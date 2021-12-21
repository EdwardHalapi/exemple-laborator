﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Exemple.Domain;
using Exemple.Domain.Repositories;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using Example.Api.Models;
using Exemple.Domain.Models;

namespace Example.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PriceController : ControllerBase
    {
        private ILogger<PriceController> logger;

        public PriceController(ILogger<PriceController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderLines([FromServices] IOrderLineRepository orderLineRepository) =>
            await orderLineRepository.TryGetExistingProduct().Match(
               Succ: GetAllProductsHandleSuccess,
               Fail: GetAllProductsHandleError
            );
        private ObjectResult GetAllProductsHandleError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return base.StatusCode(StatusCodes.Status500InternalServerError, "UnexpectedError");
        }

        private OkObjectResult GetAllProductsHandleSuccess(List<Exemple.Domain.Models.CalculatedCustomerPrice> price) =>
        Ok(price.Select(price => new
        {
            ProductCode = price.ProductCode.Value,
            price.Quantity,
            price.price,
        }));

        [HttpPost]
        public async Task<IActionResult> PaidProducts([FromServices] PublishProductWorkflow publishProductWorkflow, [FromBody] InputProduct[] products)
        {
            var unvalidatedProducts = products.Select(MapInputProductToValidateddProduct)
                                          .ToList()
                                          .AsReadOnly();
            PublishQuantityCommand command = new(unvalidatedProducts);
            var result = await publishProductWorkflow.ExecuteAsync(command);
            return result.Match<IActionResult>(
                whenPaidCaruciorFaildEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                whenPaidCaruciorScucceededEvent: successEvent => Ok()
            );
        }
        [HttpPost]
        public async Task<IActionResult> PlaceItemToOrder([FromServices] PublishProductWorkflow publishProductWorkflow, [FromBody] InputOrderLine[] products)
        {
            var unvalidatedProducts = products.Select(MapInputProductToUnvalidatedProduct)
                                          .ToList()
                                          .AsReadOnly();
            PublishQuantityCommand command = new(unvalidatedProducts);
            var result = await publishProductWorkflow.ExecuteAsync(command);
            return result.Match<IActionResult>(
                whenPaidCaruciorFaildEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                whenPaidCaruciorScucceededEvent: successEvent => Ok()
            );
        }
        private static UnvalidatedProductQuantity MapInputProductToUnvalidatedProduct(InputOrderLine product) => new UnvalidatedProductQuantity(
          cod: product.OrderLineId.ToString(),
            quantity: product.Quantity,
            address: product.Price.ToString());
        private static UnvalidatedProductQuantity MapInputProductToValidateddProduct(InputProduct product) => new UnvalidatedProductQuantity(
             cod: product.Code,
            quantity: product.Stoc,
            address: product.ProductCodeId.ToString());
    }
}
