using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HardwareOrder.ApiController.Models
{
    [Serializable]
    public class OrderItemModel
    {   
        [Required]
        public int DeviceId { get; set; }
        public DeviceModel Device { get; set; }
        [Required]
        public int Quantity { get; set; }
        public decimal OrderItemPrice { get; set; }
    }
}
