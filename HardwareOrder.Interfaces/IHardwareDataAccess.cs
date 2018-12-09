using HardwareOrder.Dataaccess.Models;
using HardwareOrder.Models.ApiController.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HardwareOrder.Interfaces
{
    public interface IHardwareDataAccess
    {
        List<Order> GetOrders();
        BusinessResult AddOrder(Order order);
    }
}
