using AutoMapper;
using ShoppingApi.Data;
using ShoppingApi.Models;
using System;
using System.Linq;

namespace ShoppingApi
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<ShoppingItem, ShoppingListItemResponse>();
            CreateMap<CreateCurbisdeOrder, OrderForCurbside>()
                .ForMember(dest =>  dest.Items, opt => opt.MapFrom(src => string.Join(",", src.Items)));
            CreateMap<OrderForCurbside, CurbsideOrder>()
             .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.Split(',', StringSplitOptions.None).ToList()));
        }
    }
}
