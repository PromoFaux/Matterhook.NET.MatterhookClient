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

            Assert.Equal(6, chunks.Count);
            Assert.Equal("Before\n```json\none\n```", chunks[0]);
            Assert.Equal("```json\ntwo three\n```", chunks[1]);
            Assert.Equal("```json\nfour five six\n```", chunks[2]);
            Assert.Equal("```json\nseven eight\n```", chunks[3]);
            Assert.Equal("```json\nnine\n```", chunks[4]);
            Assert.Equal("```json\nten\n```\nAfter", chunks[5]);
        }

        [Fact]
        public void StringSplitterKeepsFencedChunksWithinMaxChunkSize()
        {
            var text = "Before\n```json\none two three four five six seven eight nine ten\n```\nAfter";
            const int maxChunkSize = 25;

            var chunks = StringSplitter.SplitTextIntoChunks(text, maxChunkSize).ToList();

            Assert.All(chunks, chunk => Assert.True(chunk.Length <= maxChunkSize,
                $"Chunk '{chunk}' ({chunk.Length} chars) exceeds maxChunkSize ({maxChunkSize})."));
        }

        [Fact]
        public void StringSplitterTruncatesToTheFirstChunk()
        {
            var chunks = StringSplitter.SplitTextIntoChunks("one two three four", 7, truncate: true).ToList();

            Assert.Single(chunks);
            Assert.Equal("one two", chunks[0]);

            var markdownChunks = StringSplitter.SplitTextIntoChunks("Before\n```json\none two three four\n```", 30, truncate: true).ToList();

            Assert.Single(markdownChunks);
            Assert.Equal("Before\n```json\none two\n```", markdownChunks[0]);
        }

    }
}
