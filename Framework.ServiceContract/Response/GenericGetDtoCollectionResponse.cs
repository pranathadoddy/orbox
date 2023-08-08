using System.Collections.Generic;

namespace Framework.ServiceContract.Response
{
    public class GenericGetDtoCollectionResponse<TDto> : BasicResponse
    {
        #region Fields

        private ICollection<TDto> _dtoList;

        #endregion

        #region Properties

        public ICollection<TDto> DtoCollection
        {
            get => _dtoList ?? (_dtoList = new List<TDto>());

            set => _dtoList = value;
        }

        #endregion
    }
}
