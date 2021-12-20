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
    public interface IPublishProductWorkflow
    {
        public Task<IPaidCarucioredEvent> ExecuteAsync(PublishQuantityCommand command);
        public Task<Either<ICarucior, PaidCarucior>> ExecuteWorkflowAsync(UnvalidatedProduct unvalidatedProducts,
                                                                                         IEnumerable<CalculatedCustomerPrice> existingprods,
                                                                                         Func<ProductCode, Option<ProductCode>> checkProdExists);

    }
}
