using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Full.Pirate.Library.Models
{
    public class AuthorToCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }

        public ICollection<BookToCreateDto> Books { get; set; }
        = new List<BookToCreateDto>();


    }
}
