using System.ComponentModel.DataAnnotations;

namespace Framework.Application.Validation
{
    public class IntegerNotEqualToValidation : ValidationAttribute
    {
        private readonly int _targetValue;

        public IntegerNotEqualToValidation(int targetValue)
        {
            _targetValue = targetValue;
        }

        public override bool IsValid(object value)
        {
            return (int) value != _targetValue;
        }
    }
}