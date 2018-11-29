using System;
namespace Core
{
    public static class Message
    {
        public enum Warning
        {
            OutOfStock,
            OutOfDocumentRange,
            ExceedesCreditLimit
        }

        public enum Success
        {
            Success,
            SuccessButNothingDone
        }
    }
}
