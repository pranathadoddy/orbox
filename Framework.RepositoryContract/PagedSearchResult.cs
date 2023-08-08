using System.Collections.Generic;

namespace Framework.RepositoryContract
{
    public class PagedSearchResult<TDto>
    {
        #region Fields

        private ICollection<TDto> _result;

        #endregion

        #region Properties

        public int Count { get; set; }

        public ICollection<TDto> Result
        {
            get { return this._result ?? (this._result = new List<TDto>()); }

            set { this._result = value; }
        }

        #endregion
    }
}
