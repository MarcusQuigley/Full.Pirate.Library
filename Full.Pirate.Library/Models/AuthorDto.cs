using System;

namespace Full.Pirate.Library.Models
{
    public class AuthorDto
    { 
        public Guid AuthorId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string MainCategory { get; set; }
    }
}
