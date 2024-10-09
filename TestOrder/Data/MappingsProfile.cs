using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestOrder.Models;

namespace TestOrder.Data
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderViewModel, Order>();
            CreateMap<Item, ItemViewModel>().ReverseMap();
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
        }
    }
}
