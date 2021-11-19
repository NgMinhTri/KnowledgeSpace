using System.Collections.Generic;

namespace KnowledgeSpace.ViewModel
{
    public class Pagination<T> : PaginationBase where T: class
    {
        public List<T> Items { get; set; }
    }
}
