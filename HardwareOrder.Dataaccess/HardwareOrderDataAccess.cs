using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HardwareOrder.Dataaccess.Models;
using HardwareOrder.Interfaces;
using HardwareOrder.Models.ApiController.Models;
using Newtonsoft.Json;

namespace HardwareOrder.Dataaccess
{
    public class HardwareOrderDataAccess : IHardwareDataAccess
    {
        private static readonly string OrdersJsonFilePath = @"..\\HardwareOrder.DataAccess\\orders.json";
        private static readonly string DevicesJsonFilePath = @"..\\HardwareOrder.DataAccess\\devices.json";

        //Uncomment the below variables and comment the above set to run the tests
        //private static readonly string OrdersJsonFilePath = @"..\\..\\..\\..\\HardwareOrder.DataAccess\\orders.json";
        //private static readonly string DevicesJsonFilePath = @"..\\..\\..\\..\\HardwareOrder.DataAccess\\devices.json";

        /// <summary>
        /// Returns a list of orders by reading a JSON file
        /// </summary>
        /// <returns></returns>
        public List<Order> GetOrders()
        {            
            List<Order> orders = new List<Order>();
            if (File.Exists(OrdersJsonFilePath))
                orders = JsonConvert.DeserializeObject<List<Order>>(File.ReadAllText(OrdersJsonFilePath));

            return orders;
        }

        /// <summary>
        /// Adds the input order, after calculating the order item prices
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public BusinessResult AddOrder(Order order)
        {   
            #region Order validation checks
            if(order == null || order.OrderItems == null)
                return new BusinessResult(string.Empty, "Invalid Order.");
            if(order.OrderItems.Count == 0)
                return new BusinessResult(string.Empty, "No Order items found.");
            if(order.OrderItems.Any(x => x.DeviceId == 0))
                return new BusinessResult(string.Empty, "No device found");
            if (order.Address == null || string.IsNullOrEmpty(order.Address.AddressLine1))
                return new BusinessResult(string.Empty, "Invalid address.");
            if (order.Address == null || string.IsNullOrEmpty(order.CustomerName))
                return new BusinessResult(string.Empty, "Invalid Customer name.");
            #endregion Order validation checks

            #region Calculate order price
            foreach(var orderItem in order.OrderItems)
            {
                var device = GetDeviceById(orderItem.DeviceId);
                if(device == null)
                    return new BusinessResult(string.Empty, "No device found");
                var devicePrice = device != null ? device.Price : 0;
                orderItem.OrderItemPrice = orderItem.Quantity * devicePrice;
                orderItem.Device = device;
            }
            order.OrderPrice = order.OrderItems.Sum(x => x.OrderItemPrice);
            #endregion Calculate order price

            #region Write the new order into orders.json file
            //Retrieve current orders
            var existingOrders = GetOrders();
            existingOrders.Add(order);
            order.Id = existingOrders != null && existingOrders.Count > 0 ?
                existingOrders.Max(x => x.Id) + 1 : 1;
            order.CreatedDate = DateTime.Now;

            var jsonString = JsonConvert.SerializeObject(existingOrders);
            File.WriteAllText(OrdersJsonFilePath, jsonString);
            #endregion Write the new order into orders.json file

            return new BusinessResult(order.ReferenceNumber, string.Empty);
        }

        /// <summary>
        /// Gets the device details from the device id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private Device GetDeviceById(int Id)
        {
            List<Device> devices = new List<Device>();
            //Read the list of devices from Json file
            if (File.Exists(OrdersJsonFilePath))
                devices = JsonConvert.DeserializeObject<List<Device>>(File.ReadAllText(DevicesJsonFilePath));

            //Find the matching device, else return null
            return devices != null
                && devices.Count > 0
                && devices.FirstOrDefault(x => Id == x.Id) != null
                ? devices.FirstOrDefault(x => Id == x.Id)
                : null;            
        }

        /// <summary>
        /// Data set up method for initial set up of Devices. Writes into a JSON file.
        /// </summary>
        public void SetupDataDevices()
        {
            var device1 = new Device { Id = 1, Code = "mdeviceapp001", Description = "Apple iPhone 8 - 64 GB", Name = "Apple iPhone 8", Price = 998 };
            var device2 = new Device { Id = 2, Code = "mdevicesam001", Description = "Samsung Galaxy S9 - 128 GB", Name = "Samsung Galaxy S9", Price = 1254 };
            var device3 = new Device { Id = 3, Code = "mdevicemoto001", Description = "Moto G5 - 128 GB", Name = "Moto G5", Price = 459 };

            var devices = new List<Device>();
            devices.Add(device1);
            devices.Add(device2);
            devices.Add(device3);

            var jsonString = JsonConvert.SerializeObject(devices);
            File.WriteAllText(DevicesJsonFilePath, jsonString);
        }

        /// <summary>
        /// Data set up method for initial set up of Orders. Writes into a JSON file.
        /// </summary>
        private void SetupOrders()
        {
            var device1 = new Device { Id = 1, Code = "mdeviceapp001", Description = "Apple iPhone 8 - 64 GB", Name = "Apple iPhone 8", Price = 998 };
            var device2 = new Device { Id = 2, Code = "mdevicesam001", Description = "Samsung Galaxy S9 - 128 GB", Name = "Samsung Galaxy S9", Price = 1254 };
            var device3 = new Device { Id = 3, Code = "mdevicemoto001", Description = "Moto G5 - 128 GB", Name = "Moto G5", Price = 459 };

            var orderItem1 = new OrderItem {Device = device1, DeviceId = 1, Quantity = 2 };
            var orderItem2 = new OrderItem {Device = device2, DeviceId = 2, Quantity = 4 };
            var orderItem3 = new OrderItem {Device = device3, DeviceId = 3, Quantity = 5 };
            var orderItem4 = new OrderItem {Device = device2, DeviceId = 2, Quantity = 7 };

            var address1 = new Address { AttentionTo = "Test User", AddressLine1 = "92 Albert street", AddressLine2 = "Auckland Central", City = "Auckland", Country = "New Zealand", PostCode = "1010", Suburb = "Auckland Central" };

            var order1 = new Order { Address = address1, CreatedDate = DateTime.Now, CustomerName = "ANZ", Id = 1, OrderPrice = 15000};
            order1.OrderItems = new List<OrderItem>();
            order1.OrderItems.Add(orderItem1);
            order1.OrderItems.Add(orderItem2);

            var order2 = new Order { Address = address1, CreatedDate = DateTime.Now, CustomerName = "Fonterra", Id = 2, OrderPrice = 16000};
            order2.OrderItems = new List<OrderItem>();
            order2.OrderItems.Add(orderItem3);
            order2.OrderItems.Add(orderItem4);

            var orders = new List<Order>();
            orders.Add(order1);
            orders.Add(order2);

            var jsonString = JsonConvert.SerializeObject(orders);
            File.WriteAllText(OrdersJsonFilePath, jsonString);
        }
        
    }
}
