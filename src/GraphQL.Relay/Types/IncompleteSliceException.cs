using System;

namespace GraphQL.Relay.Types
{
    [Serializable]
    public class IncompleteSliceException : ArgumentException
    {
        public IncompleteSliceException() : this("The provided data slice does not contain all expected items.")
        {
        }

        public IncompleteSliceException(string message) : base(message)
        {
        }

        public IncompleteSliceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public IncompleteSliceException(string message, string paramName, Exception innerException) : base(message,
            paramName, innerException)
        {
        }

        public IncompleteSliceException(string message, string paramName) : base(message, paramName)
        {
        }
    }
}