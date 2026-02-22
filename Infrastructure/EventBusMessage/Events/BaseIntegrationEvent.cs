using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusMessages.Events
{
    public class BaseIntegrationEvent
    {

        public string CreatingId { get; set; }
    
        public DateTime CreatedDate { get; set; }

        public BaseIntegrationEvent(string creatingId, DateTime createdDate)
        {
            CreatingId = creatingId;
            CreatedDate = createdDate;
        }

        public BaseIntegrationEvent()
        {
            CreatingId=Guid.NewGuid().ToString();   
            CreatedDate=DateTime.UtcNow;
        }
    }
}
