using System;
using System.Runtime.Serialization;

namespace PA.Services
{
    [Serializable]
    internal class ProductAlreadyExists : Exception
    {
        public ProductAlreadyExists()
        {
        }

        public ProductAlreadyExists(string message) : base(message)
        {
        }

        public ProductAlreadyExists(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProductAlreadyExists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}