using Full.Pirate.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Services
{
 public interface IRepositoryService
    {
        bool AuthorExists(Guid authorId);
        Author GetAuthor(Guid authorId);
        IEnumerable<Author> GetAuthors();
       // IEnumerable<Author> GetAuthors(AuthorsResourceParameters parms);
        void AddAuthor(Author author);

        void UpdateAuthor(Author author);
        void DeleteAuthor(Author author);
        bool Save();

        int Count();
    }
}
