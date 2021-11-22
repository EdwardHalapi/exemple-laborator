using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record PublishQuantityCommand
    {
        public PublishQuantityCommand(IReadOnlyCollection<UnvalidatedProductQuantity> inputQuantity)
        {
            inputQuantity = this.inputQuantity;
        }

        public IReadOnlyCollection<UnvalidatedProductQuantity> inputQuantity { get; }
    }
}
