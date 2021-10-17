using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Matterhook.NET.MatterhookClient.Tests
{
    public class MiscTests
    {

        [Fact]
        public void StringSplitterThrowsExceptionWhenNullStringPassed()
        {
            Assert.Throws<ArgumentException>(() => StringSplitter.SplitTextIntoChunks(null, 250, false));
        }

        [Fact]
        public void StringSplitterThrowsExceptionWhenChunkSizeOfLessThan1()
        {
            Assert.Throws<ArgumentException>(() => StringSplitter.SplitTextIntoChunks("A message", 0, false));
        }

    }
}
