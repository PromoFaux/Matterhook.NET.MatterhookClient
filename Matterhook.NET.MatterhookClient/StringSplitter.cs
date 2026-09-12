using System;
using System.Collections.Generic;
using System.Text;

namespace Matterhook.NET.MatterhookClient
{
    /// <summary>
    /// Provides text splitting functionality
    /// </summary>
    public static class StringSplitter
    {
        /// <summary>
        /// Splits a text into chuncks of a given size with the option to preserve words.
        /// </summary>
        /// <param name="str">The text to be splitted.</param>
        /// <param name="maxChunkSize">Maximum size for each text chunk.</param>
        /// <param name="preserveWords">Flag indicating if words should be preserved.</param>
        /// <param name="truncate">Flag indicating if only the first chunk should be returned.</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitTextIntoChunks(string str, int maxChunkSize, bool preserveWords = true, bool truncate = false)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentException("Text can't be null or empty.", nameof(str));
            if (maxChunkSize < 1) throw new ArgumentException("Max. chunk size must be at least 1 char.", nameof(maxChunkSize));
            if (str.Length < maxChunkSize) return new List<string> { str };

            var chunks = preserveWords
                ? SplitTextBySizePreservingWords(str, maxChunkSize)
                : SplitTextBySize(str, maxChunkSize);

            return truncate ? new List<string> { chunks[0] } : chunks;
        }

        private static List<string> SplitTextBySize(string str, int maxChunkSize)
        {
            var list = new List<string>();
            for (var i = 0; i < str.Length; i += maxChunkSize)
            {
                list.Add(str.Substring(i, Math.Min(maxChunkSize, str.Length - i)));
            }
            return list;
        }

        /// <summary>
        /// Splits text into chunks no larger than maxChunkSize, preserving whole words. Any fenced
        /// code block (``` or ~~~) that a chunk boundary would otherwise land inside of is closed at
        /// the end of the chunk and re-opened (with its original language hint) at the start of the
        /// next one. The overhead of that closing/re-opening text is reserved for up front, so a
        /// chunk is only ever allowed to grow past maxChunkSize when a single word (including any
        /// fence line stuck to it) is already too big to fit on its own - the same pre-existing
        /// limit that plain word-preserving splitting has always had.
        /// </summary>
        private static List<string> SplitTextBySizePreservingWords(string str, int maxChunkSize)
        {
            var words = str.Split(' ');

            // Fence state (opening line + closing marker) as of just after each word - computed up
            // front so the packing loop below knows, before committing a word to the current chunk,
            // whether it would need to leave a fence open (and therefore reserve room to close it).
            var fenceStateAfterWord = new (string OpenLine, string CloseMarker)[words.Length];
            string openLine = null;
            string closeMarker = null;
            for (var i = 0; i < words.Length; i++)
            {
                foreach (var line in words[i].Split('\n'))
                {
                    var fence = GetFence(line);
                    if (fence == null)
                        continue;

                    if (openLine == null)
                    {
                        openLine = line;
                        closeMarker = fence;
                    }
                    else if (fence == closeMarker)
                    {
                        openLine = null;
                        closeMarker = null;
                    }
                }

                fenceStateAfterWord[i] = (openLine, closeMarker);
            }

            var result = new List<string>();
            var current = new StringBuilder();
            var stateAtEndOfCurrent = (OpenLine: (string)null, CloseMarker: (string)null);

            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];
                var stateBeforeWord = i == 0 ? (OpenLine: (string)null, CloseMarker: (string)null) : fenceStateAfterWord[i - 1];

                if (current.Length > 0)
                {
                    var stateAfterWord = fenceStateAfterWord[i];
                    var addLength = 1 + word.Length; // +1 for the joining space
                    var suffixLength = stateAfterWord.CloseMarker != null ? stateAfterWord.CloseMarker.Length + 1 : 0;

                    if (current.Length + addLength + suffixLength > maxChunkSize)
                    {
                        if (stateAtEndOfCurrent.OpenLine != null)
                            current.Append('\n').Append(stateAtEndOfCurrent.CloseMarker);
                        result.Add(current.ToString());
                        current.Clear();
                    }
                }

                if (current.Length == 0)
                {
                    if (stateBeforeWord.OpenLine != null)
                        current.Append(stateBeforeWord.OpenLine).Append('\n');
                    current.Append(word);
                }
                else
                {
                    current.Append(' ').Append(word);
                }

                stateAtEndOfCurrent = fenceStateAfterWord[i];
            }

            if (current.Length > 0)
                result.Add(current.ToString());

            return result;
        }

        private static string GetFence(string line)
        {
            var trimmedLine = line.TrimStart(' ', '\t');
            if (trimmedLine.Length < 3)
                return null;

            var character = trimmedLine[0];
            if (character != '`' && character != '~')
                return null;

            var length = 0;
            while (length < trimmedLine.Length && trimmedLine[length] == character)
                length++;

            return length >= 3 ? new string(character, length) : null;
        }
    }
}
