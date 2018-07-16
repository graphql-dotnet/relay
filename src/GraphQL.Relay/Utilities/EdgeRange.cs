using System;

namespace GraphQL.Relay.Utilities
{
    public struct EdgeRange
    {
        private int startOffset;
        private int endOffset;

        public EdgeRange(int startOffset, int endOffset)
        {
            if (startOffset < 0) throw new ArgumentOutOfRangeException(nameof(startOffset));
            if (endOffset < -1) throw new ArgumentOutOfRangeException(nameof(endOffset));
            this.startOffset = startOffset;
            this.endOffset = Math.Max(startOffset - 1, endOffset);
        }

        public int StartOffset => startOffset;

        public int EndOffset => endOffset;

        public int Count => endOffset - startOffset + 1;

        public bool IsEmpty => Count == 0;

        /// <summary>
        /// Ensures that <see cref="Count"/> is equal to or less than <paramref name="maxLength"/>
        /// by moving the end offset towards start offset. 
        /// </summary>
        /// <param name="maxLength">Maximum count</param>
        public void LimitCountFromStart(int maxLength)
        {
            if (maxLength < 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            if (maxLength < Count)
            {
                endOffset = startOffset + maxLength - 1;
            }
        }
        
        
        /// <summary>
        /// Ensures that <see cref="Count"/> is equal to or less than <paramref name="maxLength"/>
        /// by moving the start offset towards start offset. 
        /// </summary>
        /// <param name="maxLength">Maximum count</param>
        public void LimitCountToEnd(int maxLength)
        {
            if (maxLength < 0) throw new ArgumentOutOfRangeException(nameof(maxLength));
            if (maxLength < Count)
            {
                startOffset = endOffset - maxLength + 1;
            }
        }
    }
}
s