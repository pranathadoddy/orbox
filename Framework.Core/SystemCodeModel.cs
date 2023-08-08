namespace Framework.Core
{
    public class SystemCodeModel<T>
    {
        #region Constructors

        public SystemCodeModel(T value, string description, int weight = 0)
        {
            this.Value = value;
            this.Description = description;
            this.Weight = weight;
        }

        #endregion

        #region Properties

        public T Value { get; }

        public string Description { get; }

        public int Weight { get; }

        #endregion
    }
}
