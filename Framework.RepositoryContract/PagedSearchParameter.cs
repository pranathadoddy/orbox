namespace Framework.RepositoryContract
{
    public class PagedSearchParameter
    {
        #region Properties

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string OrderByFieldName { get; set; }

        public string SortOrder { get; set; }

        public string Keyword { get; set; }

        public string Filters { get; set; }

        #endregion
    }
}
