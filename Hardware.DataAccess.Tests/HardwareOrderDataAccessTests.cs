using HardwareOrder.Dataaccess;
using HardwareOrder.Dataaccess.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace HardwareOrder.DataAccess.Tests
{
    [TestFixture]
    public class HardwareOrderDataAccessTests
    {
        private readonly HardwareOrderDataAccess _hardwareOrderDataAccess;
        public HardwareOrderDataAccessTests()
        {
            _hardwareOrderDataAccess = new HardwareOrderDataAccess();
        }
       
        [Test]
        public void GetOrders_ReturnsData()
        {
            var orders = _hardwareOrderDataAccess.GetOrders();
            Assert.IsNotNull(orders);
            Assert.IsTrue(orders.Count > 0);
        }

        [Test]
        public void AddOrders_ValidationError()
        {
            var address1 = new Address { AttentionTo = "Test User", AddressLine1 = "92 Albert street", AddressLine2 = "Auckland Central", City = "Auckland", Country = "New Zealand", PostCode = "1010", Suburb = "Auckland Central" };
            var orderItem1 = new OrderItem { DeviceId = 100, Quantity = 2 };
            var order1 = new Order { Address = address1, CreatedDate = DateTime.Now, CustomerName = "ANZ", Id = 1, OrderPrice = 15000 };
            order1.OrderItems = new List<OrderItem>();
            order1.OrderItems.Add(orderItem1);

            var result = _hardwareOrderDataAccess.AddOrder(order1);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotEmpty(result.ErrorMessage);
        }

        [Test]
        public void AddOrders_Success()
        {
            var address1 = new Address { AttentionTo = "Test User", AddressLine1 = "92 Albert street", AddressLine2 = "Auckland Central", City = "Auckland", Country = "New Zealand", PostCode = "1010", Suburb = "Auckland Central" };
            var orderItem1 = new OrderItem { DeviceId = 1, Quantity = 2 };
            var order1 = new Order { Address = address1, CreatedDate = DateTime.Now, CustomerName = "ANZ", Id = 1, OrderPrice = 15000 };
            order1.OrderItems = new List<OrderItem>();
            order1.OrderItems.Add(orderItem1);

            var result = _hardwareOrderDataAccess.AddOrder(order1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotEmpty(result.OrderReferenceNumber);
        }
    }
}