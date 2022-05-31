using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.BLL.Models;
using TGWLP.DAL.Entities;

namespace TGWLP.BLL
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Story, StoryViewModel>();
            CreateMap<Story, StoryEditModel>();
            CreateMap<StoryEditModel, Story>();
        }
    }
}
