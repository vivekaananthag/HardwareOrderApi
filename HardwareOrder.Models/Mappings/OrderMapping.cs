using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HardwareOrder.ApiController.Models;
using HardwareOrder.Dataaccess.Models;

namespace HardwareOrder.Models.Mappings
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<Order, OrderModel>().ReverseMap();                
            CreateMap<OrderItem, OrderItemModel>().ReverseMap();
            CreateMap<Device, DeviceModel>().ReverseMap();
            CreateMap<Address, AddressModel>().ReverseMap();

        }
    }
}
