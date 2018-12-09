using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareOrder.Dataaccess.Models
{
    [Serializable]
    public class Address
    {   
        public string AttentionTo { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Suburb { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
    }
}
