

using System;

namespace DotnetProject.SampleApi.Application.Common
{
    /// <summary>
    /// Paging details
    /// </summary>
    public class PagingDetails
    {
        public PagingDetails()
        { }

        public PagingDetails(int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex < 0 ? 0 : pageIndex;
            PageSize = pageSize < 0 ? 0 : pageSize;
            TotalCount = totalCount < 0 ? 0 : totalCount;
        }

        /// <summary>
        /// Current page index, starting from 0
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Current page number, starting from 1
        /// </summary>
        public int PageNumber => PageIndex + 1;

        /// <summary>
        /// Items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of items
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages
        {
            get
            {
                var total = TotalCount / PageSize;

                if (TotalCount % PageSize > 0)
                    total++;

                return total;
            }
        }

        /// <summary>
        /// Previous page exists or not
        /// </summary>
        public bool HasPreviousPage => (PageIndex > 0);

        /// <summary>
        /// Next page exists or not
        /// </summary>
        public bool HasNextPage => (PageIndex + 1 < TotalPages);

        /// <summary>
        /// First item page index
        /// </summary>
        public int FirstItemIndex => (PageIndex * PageSize) + 1;

        /// <summary>
        /// Last item page index
        /// </summary>
        public int LastItemIndex => Math.Min(TotalCount, ((PageIndex * PageSize) + PageSize));

        /// <summary>
        /// Current page is first page or not
        /// </summary>
        public bool IsFirstPage => (PageIndex <= 0);

        /// <summary>
        /// Current page is last page or not
        /// </summary>
        public bool IsLastPage => (PageIndex >= (TotalPages - 1));
    }
}
