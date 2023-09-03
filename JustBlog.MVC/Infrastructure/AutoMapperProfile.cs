using AutoMapper;
using FA.JustBlog.Core.Models;
using JustBlog.MVC.Models;

namespace JustBlog.MVC.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();
        }
    }
}
