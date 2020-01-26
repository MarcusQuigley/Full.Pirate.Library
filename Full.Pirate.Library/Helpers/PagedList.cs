using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.Helpers
{
    public class PagedList<T> : List<T>
    {
        private PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => (CurrentPage >1);
        public bool HasNext => (CurrentPage < TotalPages);

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            int count = source.Count();
            var items =await source.Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize)
                        .ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            int count = source.Count();
            var items = source.Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize)
                        .ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
