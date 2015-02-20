using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IOC.FW.Core;
using IOC.FW.Core.Extensions;

namespace IOC.FW.Web.Pagination
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
            if (String.IsNullOrEmpty(url))
                url = String.Empty;

            this.Url = url;

            if (urlParams == null)
                urlParams = new Dictionary<string, object>();

            this.TotalPages = (uint)Math.Ceiling((decimal)totalItems / (decimal)itemsPerPage);
            this.UrlParams = urlParams;
            this.Page = page;
            this.TotalItems = totalItems;
            this.ItemsPerPage = itemsPerPage;
            this.PageRange = pageRange;
            this.FirstPageInRange = pageRange >= page ? 1 : page - pageRange;
            this.LastPageInRange = Math.Min(this.TotalPages, page + pageRange);
            this.PrevPage = this.Page <= 1 ? 1 : this.Page - 1;
            this.NextPage = this.Page >= this.TotalPages ? this.TotalPages : this.Page + 1;

        }

        public string GetPrevUrl()
        {
            return this.GetPageUrl(this.PrevPage);
        }

        public string GetNextUrl()
        {
            return this.GetPageUrl(this.NextPage);
        }

        public string GetCurrentPageUrl()
        {
            return this.GetPageUrl(this.Page);
        }

        public string GetPageUrl(uint i)
        {
            string queryString = this.GetQueryString(i);

            return String.Concat(this.Url, queryString);
        }

        public string GetQueryString(uint i)
        {
            string queryString = "?{0}";
            IDictionary<string, object> urlParams = new Dictionary<string, object>(this.UrlParams);
            urlParams.Add("page", i);

            return String.Format(queryString, urlParams.ToQueryString());
        }
    }
}