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
        /// <returns></returns>
        public static IEnumerable<string> SplitTextIntoChunks(string str, int maxChunkSize, bool preserveWords = true)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentException("Text can't be null or empty.", nameof(str));
            if (maxChunkSize < 1) throw new ArgumentException("Max. chunk size must be at least 1 char.", nameof(maxChunkSize));
            if (str.Length < maxChunkSize) return new List<string> { str };
            if (preserveWords)
            {
                return SplitTextBySizePreservingWords(str, maxChunkSize);
            }
            else
            {
                return SplitTextBySize(str, maxChunkSize);
            }
        }

        private static IEnumerable<string> SplitTextBySize(string str, int maxChunkSize)
        {
            if (str.Length < maxChunkSize) return new List<string> { str };
            var list = new List<string>();
            for (var i = 0; i < str.Length; i += maxChunkSize)
            {
                list.Add(str.Substring(i, Math.Min(maxChunkSize, str.Length - i)));
            }
            return list;
        }

        private static IEnumerable<string> SplitTextBySizePreservingWords(string str, int maxChunkSize)
        {
            if (str.Length < maxChunkSize) return new List<string> { str };
            var words = str.Split(' ');
            var tempString = new StringBuilder("");
            var list = new List<string>();
            foreach (var word in words)
            {
                if (word.Length + tempString.Length + 1 > maxChunkSize)
                {
                    list.Add(tempString.ToString());
                    tempString.Clear();
                }
                tempString.Append(tempString.Length > 0 ? " " + word : word);
            }
            if (tempString.Length >= 1)
                list.Add(tempString.ToString());
            return list;
        }
    }
}