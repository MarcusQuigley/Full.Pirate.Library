using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.MapperProfiles
{
    public class BookProfile : AuthorProfile
    {
        public BookProfile()
        {
            CreateMap<Entities.Book, Models.BookDto>()
                .ReverseMap()
                ;
            CreateMap<Models.BookToCreateDto, Entities.Book>()
            //    .ReverseMap()
                ;
        }
    }
}
