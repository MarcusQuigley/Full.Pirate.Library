using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library.SearchParams
{
    public class AuthorsResourceParameters
    {
        const int maxPageSize = 20;
        int pageSize=10;
        public string  MainCategory { get; set; }
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize 
        {
            get =>pageSize;
            set {
                pageSize = (value > maxPageSize)?maxPageSize:value;
            }
        } 
    }
}
