using AutoMapper;
using WebApp.Strategy.Models;
using WebApp.Strategy.ViewModels;

namespace WebApp.Strategy.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductCreateViewModel, Product>();
    }
}