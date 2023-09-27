using GraphQL.Execution;

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

        public T Get<T>(string key, T defaultValue = default) where T : class
        {
            if (!TryGetValue(key, out object value))
                return defaultValue;
            return value is Dictionary<string, object> dictionary ? dictionary.ToObject<T>() : (T)value;
        }
    }
}
