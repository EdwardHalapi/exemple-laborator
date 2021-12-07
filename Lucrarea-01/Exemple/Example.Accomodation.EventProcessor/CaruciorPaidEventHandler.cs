using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example.Events;
using Example.Events.Models;
using Example.Dto.Events;

namespace Example.Accomodation.EventProcessor
{
    internal class CaruciorPaidEventHandler : AbstractEventHandler<CaruciorPaidEvent>
    {
        public override string[] EventTypes => new string[] { typeof(CaruciorPaidEvent).Name };

        protected override Task<EventProcessingResult> OnHandleAsync(CaruciorPaidEvent eventData)
        {
            Console.WriteLine(eventData.ToString());
            return Task.FromResult(EventProcessingResult.Completed);
        }
    }
}
