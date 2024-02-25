using AutoMapper;
using TestProject.Models;

namespace TestProject.AutoMapper;

public class AppMappingProfile : Profile
{
    public AppMappingProfile()
    {
        CreateMap<UserRegisterModel, User>();
        
        CreateMap<User, UserViewModelSummary>();
        CreateMap<User, UserViewModelDetails>();
        
        CreateMap<Role, RoleViewModelSummary>();
    }
}