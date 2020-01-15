﻿using Full.Pirate.Library.DbContexts;
using Full.Pirate.Library.Entities;
using Full.Pirate.Library.SearchParams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Full.Pirate.Library.Services
{
    public class RepositoryService : IRepositoryService
    {
        readonly PirateLibraryContext context;
        public RepositoryService(PirateLibraryContext context )
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            author.AuthorId = Guid.NewGuid();
            context.Authors.Add(author);
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
             return context.Authors;
        }

        public IEnumerable<Author> GetAuthors(AuthorsResourceParameters authorParms) //string mainCategory, string searchQuery)
        {
            if (string.IsNullOrEmpty(authorParms.MainCategory) && string.IsNullOrEmpty(authorParms.SearchQuery))
            {
                return GetAuthors();
            }
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

            return query.ToList();
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
    }
}
