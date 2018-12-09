using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareOrder.Dataaccess.Models
{
    [Serializable]
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
    }
}
