using System;
using System.IO;

using log4net;

namespace Craswell.Squid.Helper
{
    /// <summary>
    /// The base squid helper.
    /// </summary>
    public abstract class SquidHelper
    {
        /// <summary>
        /// The input delimiter.
        /// </summary>
        private const char InputDelimiter = ' ';

        /// <summary>
        /// The size of the buffer.
        /// </summary>
        private const int BufferSize = 4096;

        /// <summary>
        /// The logger.
        /// </summary>
        private ILog logger;

        /// <summary>
        /// The read buffer.
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// The standard input.
        /// </summary>
        private Stream standardInput;

        /// <summary>
        /// The standard output.
        /// </summary>
        private Stream standardOutput;

        /// <summary>
        /// Initializes a new instance of the <see cref="Craswell.Squid.Helper.SquidHelper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected SquidHelper(ILog logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            this.logger = logger;
            this.standardInput = Console.OpenStandardInput();
            this.standardOutput = Console.OpenStandardOutput();
        }

        /// <summary>
        /// Start the read/response loop.
        /// </summary>
        public void Start()
        {
            do
            {
                this.buffer = new byte[BufferSize];
                this.standardInput.Read(this.buffer, 0, this.buffer.Length);

                this.ProcessInput();
            }
            while (true);
        }

        /// <summary>
        /// Encode the specified string to bytes..
        /// </summary>
        /// <param name="toEncode">The string to encode.</param>
        /// <returns>The encoded string.</returns>
        protected static byte[] Encode(string toEncode)
        {
            return Console.OutputEncoding.GetBytes(toEncode);
        }

        /// <summary>
        /// Decodes the specified bytes to a string.
        /// </summary>
        /// <param name="toDecode">To decode.</param>
        /// <returns>The decoded string.</returns>
        protected static string Decode(byte[] toDecode)
        {
            return Console.InputEncoding.GetString(toDecode);
        }

        /// <summary>
        /// Determines the result for the input passed in from Squid.
        /// </summary>
        /// <param name="squidInput">The input.</param>
        /// <returns>The result.</returns>
        protected abstract string GetResult(SquidInput squidInput);

        /// <summary>
        /// Processes the input.
        /// </summary>
        private void ProcessInput()
        {
            var squidReader = new SquidInputReader(InputDelimiter);

            var input = this.GetInput();
            this.logger.DebugFormat("Input: {0}", input);

            var squidInput = squidReader.Read(input);
            var result = this.GetResult(squidInput);

            this.logger.DebugFormat("Result: {0}", result);
            this.WriteOutput(result);
            this.WriteOutput(Environment.NewLine);
        }

        /// <summary>
        /// Writes to standard output.  Used to respond to Squid.
        /// </summary>
        /// <param name="output">The output to be written.</param>
        private void WriteOutput(string output)
        {
            var outputBuffer = Encode(output);

            this.standardOutput.Write(
                outputBuffer,
                0,
                outputBuffer.Length);
        }

        /// <summary>
        /// Gets the input from the buffer.
        /// </summary>
        /// <returns>The input.</returns>
        private string GetInput()
        {
            var input = Decode(this.buffer);

            return input.Substring(0, input.IndexOf(Environment.NewLine));
        }

        /// <summary>
        /// Logs the input received from Squid.
        /// </summary>
        /// <param name="input">The input received from Squid.</param>
        private void LogInput(string input)
        {
            this.logger.DebugFormat(
                "Input: {0}",
                input);
        }
    }
}
