using System.Collections.Generic;

namespace GraphQL.Relay.Types
{
    public class MutationInputs : Dictionary<string, object>
    {
        public MutationInputs()
        {
        }

        public MutationInputs(IDictionary<string, object> dict) : base(dict)
        {
        }

        public object Get(string key)
        {
            return this[key];
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            object value;
            if (!TryGetValue(key, out value))
                return defaultValue;

            return (T) value;
        }
    }
}