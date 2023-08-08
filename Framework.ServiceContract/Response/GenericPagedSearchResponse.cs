using System.Collections.Generic;

namespace Framework.ServiceContract.Response
{
    public class GenericPagedSearchResponse<TDto> : BasicResponse
    {
        #region Properties

        public ICollection<TDto> DtoCollection { get; set; }

        public int TotalCount { get; set; }

        #endregion
    }
}
