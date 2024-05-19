using System;
using System.Collections.Generic;
using System.Linq;


namespace DotnetProject.SampleApi.Application.Common
{
    /// <summary>
    /// List sorting details
    /// </summary>
    public class SortingDetails
    {
        /// <summary>
        /// Sorting statements
        /// </summary>
        public List<SortItem?> SortItems { get; set; }

        public SortingDetails()
        { }

        public SortingDetails(List<SortItem?> sortItems)
        {
            if (sortItems == null || !sortItems.Any())
                throw new ArgumentException(null, nameof(sortItems));

            SortItems = sortItems;
        }

        public SortingDetails(SortItem? sortItem)
        {
            if (sortItem == null)
                throw new ArgumentException(null, nameof(sortItem));

            if (sortItem.SortBy == null || !sortItem.SortBy.Any())
                throw new ArgumentException("empty", nameof(sortItem.SortBy));

            SortItems = [sortItem];
        }
    }

    /// <summary>
    /// Sorting statement with field name and sorting direction
    /// </summary>
    public class SortItem
    {
        /// <summary>
        /// Sorting field name
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Sorting direction (ascending, descending)
        /// </summary>
        public SortDirection SortDirection { get; set; }

        public SortItem()
        { }

        public SortItem(string sortBy, SortDirection sortDirection)
        {
            if (sortBy == null || !sortBy.Any())
                throw new ArgumentException(null, nameof(sortBy));

            SortBy = sortBy;
            SortDirection = sortDirection;
        }
    }
    public enum SortDirection
    {
        /// <summary>
        /// Ascending
        /// </summary>
        Asc,

        /// <summary>
        /// Descending
        /// </summary>
        Desc,
    }
}
