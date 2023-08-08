namespace Framework.Application.Presentation
{
    public class PresentationConstant
    {
        public static class ButtonType
        {
            public const string Submit = "submit";
            public const string Button = "button";
        }

        public static class TextInputAttribute
        {
            public const int MiniTextMaxLength = 8;
            public const int ShortTextMaxLength = 256;
            public const int MediumTextMaxLength = 1024;
            public const int LargeTextMaxLength = 32768;

            public const int MaxTextareaRows = 10;

            public const int MaxPriceDigitLength = 10;
        }

        public static class TextInputType
        {
            public const string Text = "text";
            public const string Password = "password";
            public const string Email = "email";
            public const string Number = "number";
        }

        public static class InputType
        {
            public const string Checkbox = "checkbox";
        }

        public static class ColorUtility
        {
            public const string Default = "Default";
            public const string Primary = "Primary";
            public const string Info = "Info";
            public const string Success = "Success";
            public const string Warning = "Warning";
            public const string Danger = "Danger";
        }

        public static class ListItemValue
        {
            public const string True = "True";
            public const string False = "False";
        }
    }
}