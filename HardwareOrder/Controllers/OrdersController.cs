using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using HardwareOrder.ApiController.Models;
using HardwareOrder.Interfaces;
using AutoMapper;
using HardwareOrder.Models.ApiController.Models;
using HardwareOrder.Dataaccess.Models;
using Newtonsoft.Json;
using Serilog;

namespace HardwareOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IHardwareDataAccess _hardwareDataAccess;
        private readonly IMapper _mapper;

        public OrdersController(IHardwareDataAccess hardwareDataAccess, IMapper mapper)
        {
            _hardwareDataAccess = hardwareDataAccess;
            _mapper = mapper;
        }

        // GET: api/Orders
        [HttpGet]
        public ActionResult<List<OrderModel>> GetOrders()
        {            
            var ordersDM = _hardwareDataAccess.GetOrders();            

            var orders = new List<OrderModel>();
            foreach (var order in ordersDM)
            {
                orders.Add(ConvertOrderDMToOrderModel(order));
            }

            if (orders == null || orders.Count == 0)
            {
                return NotFound(new BusinessResult(string.Empty, "No orders found"));
            }
            else
            {
                Log.Information("Orders retrieved successfully");
                return Ok(orders);
            }            
        }

        // POST: api/Orders
        [HttpPost]
        public IActionResult PostOrder([FromBody] OrderModel orderModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = ConvertOrderModelToOrderDM(orderModel);
            var result = _hardwareDataAccess.AddOrder(order);

            if (result != null && result.IsSuccess)
            {
                var successMessage = string.Format("Order created successfully. Order reference number is {0}", result.OrderReferenceNumber);
                Log.Information(successMessage);
                return Ok(successMessage);
            }
            else
                return BadRequest(string.Format("Error - {0}", result.ErrorMessage));
        }

        #region Mapper methods
        /// <summary>
        /// Mapper method to convert OrderModel to Order Domain model
        /// Method added since Automapper could not be made to work for child collections within the specified time
        /// Above item is a TODO, Once done, this method will no longer be required
        /// </summary>
        /// <param name="orderModel"></param>
        /// <returns></returns>
        private Order ConvertOrderModelToOrderDM(OrderModel orderModel)
        {
            var order = _mapper.Map<Order>(orderModel);            
            if (orderModel.OrderItems != null && orderModel.OrderItems.Count > 0)
            {
                foreach(var orderItem in orderModel.OrderItems)
                {
                    var orderItemDM = new OrderItem
                    {
                        DeviceId = orderItem.DeviceId,                        
                        OrderItemPrice = orderItem.OrderItemPrice,
                        Quantity = orderItem.Quantity
                    };
                    order.OrderItems.Add(orderItemDM);
                }
            }
            return order;
        }

        /// <summary>
        /// Method to convert Order domain model to OrderModel
        /// Method added since Automapper could not be made to work for child collections within the specified time
        /// Above item is a TODO, Once done, this method will no longer be required
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private OrderModel ConvertOrderDMToOrderModel(Order order)
        {
            var orderModel = _mapper.Map<OrderModel>(order);
            if (order.OrderItems != null && order.OrderItems.Count > 0)
            {
                foreach (var orderItem in order.OrderItems)
                {
                    var orderItemModel = new OrderItemModel
                    {
                        DeviceId = orderItem.DeviceId,                        
                        OrderItemPrice = orderItem.OrderItemPrice,
                        Quantity = orderItem.Quantity,
                        Device = new DeviceModel
                        {
                            Code = orderItem.Device != null ? orderItem.Device.Code : string.Empty,
                            Description = orderItem.Device != null ? orderItem.Device.Description : string.Empty,
                            Id = orderItem.Device != null ? orderItem.Device.Id : 0,
                            Name = orderItem.Device != null ? orderItem.Device.Name : string.Empty,
                            Price = orderItem.Device != null ? orderItem.Device.Price : 0,
                        }
                    };
                    orderModel.OrderItems.Add(orderItemModel);
                }
            }
            return orderModel;
        }
        #endregion

        #region Setup Input Data for testing POST
        /// <summary>
        /// This method writes into a file which contains the JSON input for testing POST methods
        /// </summary>
        public void SetupOrderModelData()
        {
            var address1 = new AddressModel { AttentionTo = "Test User 1", AddressLine1 = "74 Queen street", City = "Auckland", Country = "New Zealand", PostCode = "1010", Suburb = "Auckland Central" };

            var device1 = new DeviceModel { Id = 1, Code = "mdeviceapp001", Description = "Apple iPhone 8 - 64 GB", Name = "Apple iPhone 8", Price = 998 };
            var device2 = new DeviceModel { Id = 2, Code = "mdevicesam001", Description = "Samsung Galaxy S9 - 128 GB", Name = "Samsung Galaxy S9", Price = 1254 };

            var orderItem1 = new OrderItemModel { DeviceId = 1, Quantity = 2 };
            var orderItem2 = new OrderItemModel { DeviceId = 2, Quantity = 4 };

            var order1 = new OrderModel { Address = address1, CustomerName = "Infosys"};
            order1.OrderItems = new List<OrderItemModel>();
            order1.OrderItems.Add(orderItem1);
            order1.OrderItems.Add(orderItem2);

            var jsonString = JsonConvert.SerializeObject(order1);
            System.IO.File.WriteAllText("..\\HardwareOrder.DataAccess\\ordersModel.json", jsonString);
        }
        #endregion Setup Input Data for testing POST

    }
}