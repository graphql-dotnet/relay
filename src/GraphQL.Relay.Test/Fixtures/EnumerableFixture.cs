using GraphQL.Relay.Test.Fixtures.Models;

namespace GraphQL.Relay.Test.Fixtures
{
    public partial class EnumerableFixture : IDisposable
    {
        private readonly List<Blog> _blogs = new();

        public EnumerableFixture()
        {
            var blogs = Enumerable.Range(1, TotalCount).Select(i => new Blog
            {
                Name = $"Blog{i}",
                Url = $"http://sample.com/{i}"
            });

            _blogs.AddRange(blogs);
        }

        public void Dispose()
        {
            _blogs.Clear();
            GC.SuppressFinalize(this);
        }

        public int TotalCount => 1000;

        public IList<Blog> Blogs => _blogs;
    }
}
