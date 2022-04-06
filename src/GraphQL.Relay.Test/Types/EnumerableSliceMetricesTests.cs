using System.Collections.Generic;
using GraphQL.Relay.Types;
using Xunit;
using static GraphQL.Relay.Test.Types.ResolveConnectionContextFactory;

namespace GraphQL.Relay.Test.Types
{
    public class EnumerableSliceMetricesTests
    {
        public static IEnumerable<object[]> GetTestData()
        {
            yield return BuildTestData(
                first: null,
                after: null,
                last: null,
                before: null,
                totalCount: 4,
                sliceCount: 4,
                startIndex: 0,
                hasNextPage: false,
                hasPreviousPage: false
            );

            yield return BuildTestData(
                first: 1,
                after: null,
                last: null,
                before: null,
                totalCount: 4,
                sliceCount: 1,
                startIndex: 0,
                hasNextPage: true,
                hasPreviousPage: false
            );

            yield return BuildTestData(
                first: null,
                after: null,
                last: 2,
                before: null,
                totalCount: 4,
                sliceCount: 2,
                startIndex: 2,
                hasNextPage: false,
                hasPreviousPage: true
            );

            yield return BuildTestData(
                first: 2,
                after: ConnectionUtils.OffsetToCursor(0),
                last: null,
                before: null,
                totalCount: 4,
                sliceCount: 2,
                startIndex: 1,
                hasNextPage: true,
                hasPreviousPage: true
            );

            yield return BuildTestData(
                first: null,
                after: null,
                last: 2,
                before: ConnectionUtils.OffsetToCursor(3),
                totalCount: 4,
                sliceCount: 2,
                startIndex: 1,
                hasNextPage: true,
                hasPreviousPage: true
            );

            yield return BuildTestData(
                first: 2,
                after: null,
                last: null,
                before: ConnectionUtils.OffsetToCursor(3),
                totalCount: 4,
                sliceCount: 2,
                startIndex: 0,
                hasNextPage: true,
                hasPreviousPage: false
            );

            yield return BuildTestData(
                first: 2,
                after: null,
                last: null,
                before: ConnectionUtils.OffsetToCursor(1),
                totalCount: 4,
                sliceCount: 1,
                startIndex: 0,
                hasNextPage: true,
                hasPreviousPage: false
            );

            yield return BuildTestData(
                first: null,
                after: null,
                last: 2,
                before: ConnectionUtils.OffsetToCursor(1),
                totalCount: 4,
                sliceCount: 1,
                startIndex: 0,
                hasNextPage: true,
                hasPreviousPage: false
            );

            yield return BuildTestData(
                first: null,
                after: null,
                last: 3,
                before: ConnectionUtils.OffsetToCursor(2),
                totalCount: 4,
                sliceCount: 2,
                startIndex: 0,
                hasNextPage: true,
                hasPreviousPage: false
            );

            yield return BuildTestData(
                first: null,
                after: null,
                last: 3,
                before: ConnectionUtils.OffsetToCursor(1),
                totalCount: 4,
                sliceCount: 1,
                startIndex: 0,
                hasNextPage: true,
                hasPreviousPage: false
            );

            yield return BuildTestData(
                first: 3,
                after: null,
                last: null,
                before: ConnectionUtils.OffsetToCursor(0),
                totalCount: 4,
                sliceCount: 0,
                startIndex: 0,
                hasNextPage: true,
                hasPreviousPage: false
            );

            yield return BuildTestData(
                first: 3,
                after: null,
                last: null,
                before: ConnectionUtils.OffsetToCursor(2),
                totalCount: 4,
                sliceCount: 2,
                startIndex: 0,
                hasNextPage: true,
                hasPreviousPage: false
            );

            yield return BuildTestData(
                first: 5,
                after: null,
                last: null,
                before: null,
                totalCount: 4,
                sliceCount: 4,
                startIndex: 0,
                hasNextPage: false,
                hasPreviousPage: false
            );
        }

        [Theory]
        [MemberData(nameof(GetTestData))]
        public void Create_WhenCreateFromFullListAndContext_ReturnsSlice(
            int? first,
            string after,
            int? last,
            string before,
            int totalCount,
            int sliceSize,
            int startIndex,
            bool hasNext,
            bool hasPrevious
        )
        {
            // Arrange
            var list = new int?[] { 1, 2, 3, 4 };

            // Act
            var slice = SliceMetrics.Create(list, CreateContext(
                first,
                after,
                last,
                before
            ));

            // Assert
            Assert.Equal(totalCount, slice.TotalCount);
            Assert.Equal(sliceSize, slice.SliceSize);
            Assert.Equal(startIndex, slice.StartIndex);
            Assert.Equal(hasNext, slice.HasNext);
            Assert.Equal(hasPrevious, slice.HasPrevious);
        }

        static object[] BuildTestData(
            int? first,
            string after,
            int? last,
            string before,
            int totalCount,
            int? sliceCount,
            int? startIndex,
            bool hasPreviousPage,
            bool hasNextPage
        ) => new object[] {
            first,
            after,
            last,
            before,
            totalCount,
            sliceCount,
            startIndex,
            hasNextPage,
            hasPreviousPage
        };
    }
}
