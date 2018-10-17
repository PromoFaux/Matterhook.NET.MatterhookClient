using System;
using System.Collections.Generic;
using System.Text;


namespace Matterhook.NET.MatterhookClient
{
    /// <summary>
    /// Provides text splitting functionality
    /// </summary>
    public class StringSplitter
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
            if (string.IsNullOrEmpty(str)) throw new ArgumentException("Text can't be null or empty",nameof(str));
            if (str.Length < maxChunkSize) yield return str;
            else if (preserveWords)
            {
                //Less Simple
                foreach (var chunk in SplitTextBySizePreservingWords(str, maxChunkSize))
                    yield return chunk;
            }
            else
            {
                //Simple
                foreach (var chunk in SplitTextBySize(str, maxChunkSize))
                    yield return chunk;
            }
        }

        private static IEnumerable<string> SplitTextBySize(string str, int maxChunkSize)
        {
            if (str.Length < maxChunkSize) yield return str;
            for (var i = 0; i < str.Length; i += maxChunkSize)
            {
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
            }
        }

        private static IEnumerable<string> SplitTextBySizePreservingWords(string str, int maxChunkSize)
        {
            if (str.Length < maxChunkSize) yield return str;
            var words = str.Split(' ');
            var tempString = new StringBuilder("");

            foreach (var word in words)
            {
                if (word.Length + tempString.Length + 1 > maxChunkSize)
                {
                    yield return tempString.ToString();
                    tempString.Clear();
                }

                tempString.Append(tempString.Length > 0 ? " " + word : word);
            }
            yield return tempString.ToString();
        }
    }
}