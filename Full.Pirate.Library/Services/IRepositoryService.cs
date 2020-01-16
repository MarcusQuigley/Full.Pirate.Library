using Full.Pirate.Library.Entities;
using Full.Pirate.Library.SearchParams;
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
        IEnumerable<Author> GetAuthors(AuthorsResourceParameters authorParms);//string mainCategory, string searchQuery);
        // IEnumerable<Author> GetAuthors(AuthorsResourceParameters parms);
        void AddAuthor(Author author);

        void UpdateAuthor(Author author);
        void DeleteAuthor(Author author);


        Book GetBook(Guid authorId, Guid bookId);
        IEnumerable<Book> GetBooks(Guid authorId);
        void AddBook(Guid authorId, Book book);
        void UpdateBook(Book book);
        void DeleteBook(Book book);


        bool Save();

        int Count();
    }
}
