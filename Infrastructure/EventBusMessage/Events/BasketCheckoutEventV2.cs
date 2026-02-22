using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusMessages.Events
{
    public class BasketCheckoutEventV2: BaseIntegrationEvent
    {

        public string UserName { get; set; }

        public decimal TotalaPrice { get; set; }
    }
}
