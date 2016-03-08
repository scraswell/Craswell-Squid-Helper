using System;

namespace Craswell.Squid.Helper
{
    /// <summary>
    /// Squid input.
    /// </summary>
    public class SquidInput
    {
        /// <summary>
        /// Gets or sets the channel identifier.
        /// </summary>
        /// <value>The channel identifier.</value>
        public int? ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; set; }
    }
}
