using System;
using System.Linq;

namespace Craswell.Squid.Helper
{
    /// <summary>
    /// Squid input reader.
    /// </summary>
    public class SquidInputReader
    {
        /// <summary>
        /// The field delimiter.
        /// </summary>
        private char fieldDelimiter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Craswell.Squid.Helper.SquidInputReader"/> class.
        /// </summary>
        /// <param name="fieldDelimiter">Field delimiter.</param>
        public SquidInputReader(char fieldDelimiter)
        {
            this.fieldDelimiter = fieldDelimiter;
        }

        /// <summary>
        /// Read the specified input.
        /// </summary>
        /// <param name="input">The input from Squid.</param>
        /// <returns>A SquidInput object initialized from the parsed text.</returns>
        public SquidInput Read(string input)
        {
            string[] parts = this.SplitInput(input);
            int? channel = this.GetChannel(parts);
            string data = string.Empty;

            if (channel.HasValue)
            {
                data = parts
                    .Skip(1)
                    .Aggregate((p, n) => string.Concat(p, " ", n));
            }
            else
            {
                data = input;
            }

            return new SquidInput()
            {
                ChannelId = channel,
                Data = data
            };
        }

        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <returns>The channel.</returns>
        /// <param name="parts">The parts of the Squid Input.</param>
        private int? GetChannel(string[] parts)
        {
            int channel;

            if (int.TryParse(parts[0], out channel))
            {
                return channel;
            }

            return null;
        }

        /// <summary>
        /// Split the specified input.
        /// </summary>
        /// <param name="input">The Squid input.</param>
        /// <returns>The Squid Input split on spaces.</returns>
        private string[] SplitInput(string input)
        {
            var splitChars = new char[] { this.fieldDelimiter };
            var options = StringSplitOptions.RemoveEmptyEntries;

            return input.Split(splitChars, options);
        }
    }
}
