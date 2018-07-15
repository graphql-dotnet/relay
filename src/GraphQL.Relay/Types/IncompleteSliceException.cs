using System;

namespace GraphQL.Relay.Types
{
    public class IncompleteSliceException : Exception
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
    }
}