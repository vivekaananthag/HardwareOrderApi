using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HardwareOrder.ApiController.Models
{
    [Serializable]
    public class OrderModel
    {   
        IList<OrderItemModel> _orderItems = null;
        public OrderModel()
        {
            _orderItems = new List<OrderItemModel>();
        }
        [Required]
        public string CustomerName { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public AddressModel Address { get; set; }
        public decimal OrderPrice { get; set; }
        public IList<OrderItemModel> OrderItems {
            get { return _orderItems; }
            set { value = _orderItems; }
        }        
    }
}
