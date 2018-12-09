using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareOrder.ApiController.Models
{
    [Serializable]
    public class DeviceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
    }
}
