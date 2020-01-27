//#undef TESTING_BANDWIDTH
using Full.Pirate.Library.DbContexts;
using Full.Pirate.Library.Entities;
using Full.Pirate.Library.Helpers;
using Full.Pirate.Library.SearchParams;
using Full.Pirate.Library.Services.Sorting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Services
{
    public class RepositoryService : IRepositoryService, IDisposable
    {
        PirateLibraryContext context;
        readonly IPropertyMappingService propertyMappingService;
        public RepositoryService(PirateLibraryContext context , IPropertyMappingService propertyMappingService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            author.AuthorId = Guid.NewGuid();
            foreach (var book in author.Books)
            {
                book.Id = Guid.NewGuid();
            }
            context.Authors.Add(author);
        }

        public void AddBook(Guid authorId, Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }
            var author = GetAuthor(authorId);
            if (author == null)
            {
                return;
            }
            book.AuthorId = authorId;
         
            context.Books.Add(book);
        }

        public bool AuthorExists(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }
            return context.Authors.Any(a => a.AuthorId == authorId);
        }

        public int Count()
        {
            return context.Authors.Count();
        }

        public void DeleteAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            context.Authors.Remove(author);
        }

        public void DeleteBook(Book book)
        {
           if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            context.Books.Remove(book);
        }
        public async Task<Author> GetAuthorAsync(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }
            return  await context.Authors.SingleOrDefaultAsync(a => a.AuthorId == authorId);
        }
        public Author GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }
            return context.Authors.SingleOrDefault(a => a.AuthorId == authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
#if TESTING_BANDWIDTH

            context.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:02';");
#endif
            return context.Authors;
        }
        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
#if TESTING_BANDWIDTH
            await context.Database.ExecuteSqlRawAsync("WAITFOR DELAY '00:00:02';");
#endif
            return await context.Authors.ToArrayAsync();
        }

        public  PagedList<Author> GetAuthors(AuthorsResourceParameters authorParms)
        {
            var query = context.Authors as IQueryable<Author>;

            if (!string.IsNullOrEmpty(authorParms.MainCategory))
            {
                query = query.Where(a => a.MainCategory == authorParms.MainCategory.Trim());
            }
            if (!string.IsNullOrEmpty(authorParms.SearchQuery))
            {
                var searchQuery = authorParms.SearchQuery.Trim();
                query = query.Where(a => a.MainCategory.Contains(searchQuery)
                                        || a.FirstName.Contains(searchQuery)
                                        || a.LastName.Contains(searchQuery)
                                    );
            }

            if (!string.IsNullOrEmpty(authorParms.OrderBy))
            {
                var mappingDetails = propertyMappingService.GetPropertyMapping<Models.AuthorDto, Author>();
                query = query.CreateSort(mappingDetails, authorParms.OrderBy);
            }

            return  PagedList<Author>.Create(query,
                authorParms.PageNumber,
                authorParms.PageSize);
        }

        public async Task<PagedList<Author>> GetAuthorsAsync(AuthorsResourceParameters authorParms)
        {
            
            var query = context.Authors as IQueryable<Author>;

            if (!string.IsNullOrEmpty(authorParms.MainCategory))
            {
                query = query.Where(a => a.MainCategory == authorParms.MainCategory.Trim());
            }
            if (!string.IsNullOrEmpty(authorParms.SearchQuery))
            {
                var searchQuery = authorParms.SearchQuery.Trim();
                query = query.Where(a => a.MainCategory.Contains(searchQuery)
                                        || a.FirstName.Contains(searchQuery)
                                        || a.LastName.Contains(searchQuery)
                                    );
            }

            if (!string.IsNullOrEmpty(authorParms.OrderBy))
            {
                var mappingDetails = propertyMappingService.GetPropertyMapping<Models.AuthorDto, Author>();
                query = query.CreateSort(mappingDetails, authorParms.OrderBy);
            }

            return await PagedList<Author>.CreateAsync(query,
                authorParms.PageNumber,
                authorParms.PageSize);
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            if (authorIds == null)
            {
                throw new ArgumentNullException(nameof(authorIds));
            }
            var authors = authorIds.Select(id => context.Authors.SingleOrDefault(i => i.AuthorId == id))
                                    .ToArray();
            return authors;
        }

        public Book GetBook(Guid authorId, Guid bookId)
        {
           if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }
            if (bookId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bookId));
            }
 
             return context.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> GetBooks(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }
            return context.Books.Where(book => book.AuthorId == authorId);
        }

        public bool Save()
        {
           return(context.SaveChanges() >=0);
        }

        public void UpdateAuthor(Author author)
        {
            //if (author == null)
            //{
            //    throw new ArgumentNullException(nameof(author));
            //}
            //if (AuthorExists(author.AuthorId))
            //{
            //    return;
            //}
            //context.Authors.Update(author);
        }

        public void UpdateBook(Book book)
        {
          //  throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) {

                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }
    }
}
