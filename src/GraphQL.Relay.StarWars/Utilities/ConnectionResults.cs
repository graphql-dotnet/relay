using System.Collections.Generic;

namespace GraphQL.Relay.StarWars.Utilities
{
    public static class ConnectionResults
    {
        public static ConnectionResults<TSource> Create<TSource>(
            IEnumerable<TSource> results,
            int totalCount
        ) => new(results, totalCount);
    }

    public class ConnectionResults<TSource>
    {
        public ConnectionResults(IEnumerable<TSource> results, int totalCount)
        {
            Results = results;
            TotalCount = totalCount;
        }

        public int TotalCount { get; }
        public IEnumerable<TSource> Results { get; }
    }
}