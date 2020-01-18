using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.MapperProfiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Entities.Author, Models.AuthorDto>()
                .ForMember(
                    dest => dest.Name,
                    source => source.MapFrom(s => $"{s.FirstName} {s.LastName}")
                )
                .ForMember(
                    dest=>dest.Age,
                    source => source.MapFrom(s=>s.DateOfBirth.GetAge())
                );

            CreateMap<Models.AuthorToCreateDto, Entities.Author>()
                .ReverseMap();
        }
    }
}
