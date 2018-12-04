
using AgileTrackingSystem.Data.Entities;
using AgileTrackingSystem.ViewModels;
using AutoMapper;

namespace AgileTrackingSystem.Data
{
    public class DBMappingProfile : Profile
    {
        public DBMappingProfile()
        {
            CreateMap<Order, OrderViewModel>()
                .ReverseMap();

            CreateMap<OrderItem, OrderItemViewModel>()
                .ReverseMap();
            
        }
    }
}
