

using System.Collections.Generic;
using System.Linq;

namespace DotnetProject.SampleApi.Application.Common
{
    /// <summary>
    /// Item list with paging details
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class PagedList<TItem>
    {
        /// <summary>
        /// Paging details
        /// </summary>
        public PagingDetails PagingDetails { get; set; }

        /// <summary>
        /// List items
        /// </summary>
        public List<TItem>? List { get; set; }

        public PagedList(IQueryable<TItem> source, int pageIndex, int pageSize)
        {
            PagingDetails = new PagingDetails(pageIndex, pageSize, source?.Count() ?? 0);
            List = source?.Skip(PagingDetails.PageIndex * PagingDetails.PageSize).Take(PagingDetails.PageSize).ToList();
        }

        public PagedList(IEnumerable<TItem> source, int pageIndex, int pageSize, int totalCount)
        {
            PagingDetails = new PagingDetails(pageIndex, pageSize, totalCount);
            List = source.ToList();
        }

        public PagedList(IEnumerable<TItem> source, PagingDetails pageDetails)
        {
            PagingDetails = pageDetails;
            List = source.ToList();

        }
    }
}
