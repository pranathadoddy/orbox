using System.Collections.Generic;
using System.Linq;

namespace Framework.Core
{
    public abstract class SystemCodeBase<T>
    {
        #region Fields

        protected List<SystemCodeModel<T>> CodeList;

        #endregion

        #region Public Methods

        public string GetDescription(T code)
        {
            return this.CodeList.Single(item => EqualityComparer<T>.Default.Equals(item.Value, code)).Description;
        }

        public Dictionary<T, string> ToDictionary()
        {
            return this.CodeList.ToDictionary(item => item.Value, item => item.Description);
        }

        public List<SystemCodeModel<T>> GetCodeList()
        {
            return this.CodeList;
        }

        public SystemCodeModel<T> GetItem(T code)
        {
            return this.CodeList.Single(item => EqualityComparer<T>.Default.Equals(item.Value, code));
        }

        #endregion
    }
}
