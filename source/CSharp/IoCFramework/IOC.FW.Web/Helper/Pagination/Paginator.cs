using IOC.FW.Extensions;
using System;
using System.Collections.Generic;

namespace IOC.FW.Web.Helper.Pagination
{
    //TODO: Paginar itens de list de itens também
    public class Paginator
    {
        public readonly string Url;
        public readonly IDictionary<string, object> UrlParams;
        public readonly uint Page;
        public readonly uint ItemsPerPage;
        public readonly uint PrevPage;
        public readonly uint NextPage;
        public readonly uint TotalItems;
        public readonly uint TotalPages;
        public readonly uint PageRange;
        public readonly uint FirstPageInRange;
        public readonly uint LastPageInRange;

        public Paginator(uint page, uint totalItems, uint itemsPerPage, uint pageRange, string url = null, IDictionary<string, object> urlParams = null)
        {
            if (string.IsNullOrEmpty(url))
                url = string.Empty;

            Url = url;

            if (urlParams == null)
                urlParams = new Dictionary<string, object>();

            TotalPages = (uint)Math.Ceiling((decimal)totalItems / itemsPerPage);
            UrlParams = urlParams;
            Page = page;
            TotalItems = totalItems;
            ItemsPerPage = itemsPerPage;
            PageRange = pageRange;
            FirstPageInRange = pageRange >= page ? 1 : page - pageRange;
            LastPageInRange = Math.Min(TotalPages, page + pageRange);
            PrevPage = Page <= 1 ? 1 : Page - 1;
            NextPage = Page >= TotalPages ? TotalPages : Page + 1;

        }

        public string GetPrevUrl()
        {
            return GetPageUrl(PrevPage);
        }

        public string GetNextUrl()
        {
            return GetPageUrl(NextPage);
        }

        public string GetCurrentPageUrl()
        {
            return GetPageUrl(Page);
        }

        public string GetPageUrl(uint i)
        {
            string queryString = GetQueryString(i);

            return string.Concat(Url, queryString);
        }

        public string GetQueryString(uint i)
        {
            string queryString = "?{0}";
            IDictionary<string, object> urlParams = new Dictionary<string, object>(UrlParams);
            urlParams.Add("page", i);

            return string.Format(queryString, urlParams.ToQueryString());
        }
    }
}