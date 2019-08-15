using System;

namespace GraphQL.Relay.Utilities
{
    public struct EdgeRange
    {
        public EdgeRange(int startOffset, int endOffset)
        {
            if (startOffset < 0) throw new ArgumentOutOfRangeException(nameof(startOffset));
            if (endOffset < -1) throw new ArgumentOutOfRangeException(nameof(endOffset));
            StartOffset = startOffset;
            EndOffset = Math.Max(startOffset - 1, endOffset);
        }

        public int StartOffset { get; private set; }

        public int EndOffset { get; private set; }

        public int Count => EndOffset - StartOffset + 1;

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
                EndOffset = StartOffset + maxLength - 1;
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
                StartOffset = EndOffset - maxLength + 1;
            }
        }
    }
}
