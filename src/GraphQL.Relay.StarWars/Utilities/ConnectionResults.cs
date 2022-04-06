namespace GraphQL.Relay.StarWars.Utilities
{
    public static class ConnectionEntities
    {
        public static ConnectionEntities<TSource> Create<TSource>(
            IEnumerable<TSource> entities,
            int totalCount
        ) => new(entities, totalCount);
    }

    public class ConnectionEntities<TSource>
    {
        public ConnectionEntities(IEnumerable<TSource> entities, int totalCount)
        {
            Entities = entities;
            TotalCount = totalCount;
        }

        public int TotalCount { get; }
        public IEnumerable<TSource> Entities { get; }
    }
}
