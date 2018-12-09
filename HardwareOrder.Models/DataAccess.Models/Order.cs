using System;
using System.Collections.Generic;
using System.Text;

namespace HardwareOrder.Dataaccess.Models
{
    [Serializable]
    public class Order
    {
        public int Id { get; set; }
        IList<OrderItem> _orderItems = null;
        public Order()
        {
            _orderItems = new List<OrderItem>();
        }
        public string CustomerName { get; set; }
        public string ReferenceNumber { get { return string.Format("ORDER-REF-{0}", Id); } }
        public DateTime CreatedDate { get; set; }
        public Address Address { get; set; }
        public IList<OrderItem> OrderItems {
            get { return _orderItems; }
            set { value = _orderItems; }
        }
        public decimal OrderPrice { get; set; }
    }
}
