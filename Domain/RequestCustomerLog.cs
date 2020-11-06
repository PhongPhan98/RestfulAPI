using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class RequestCustomerLog
    {
        public string Action { get; set; }
        public string log { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}
