using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [Fact]
        public void StringSplitterPreservesFencedCodeBlocksAcrossChunks()
        {
            var text = "Before\n```json\none two three four five six seven eight nine ten\n```\nAfter";

            var chunks = StringSplitter.SplitTextIntoChunks(text, 25).ToList();

            Assert.Equal(3, chunks.Count);
            Assert.Equal("Before\n```json\none two\n```", chunks[0]);
            Assert.Equal("```json\nthree four five six seven\n```", chunks[1]);
            Assert.Equal("```json\neight nine ten\n```\nAfter", chunks[2]);
        }

        [Fact]
        public void StringSplitterTruncatesToTheFirstChunk()
        {
            var chunks = StringSplitter.SplitTextIntoChunks("one two three four", 7, truncate: true).ToList();

            Assert.Single(chunks);
            Assert.Equal("one two", chunks[0]);

            var markdownChunks = StringSplitter.SplitTextIntoChunks("Before\n```json\none two three four\n```", 18, truncate: true).ToList();

            Assert.Single(markdownChunks);
            Assert.Equal("Before\n```json\none\n```", markdownChunks[0]);
        }

    }
}
