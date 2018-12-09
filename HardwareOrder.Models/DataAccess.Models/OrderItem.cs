using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareOrder.Dataaccess.Models
{
    [Serializable]
    public class OrderItem
    {   
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int Quantity { get; set; }
        public decimal OrderItemPrice { get; set; }
    }
}
