using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.SharedTypes
{
    /// <summary>
    /// Channel class that crosses the DB -> Business Logic -> UI layers.
    /// </summary>
    public class SharedChannel
    {
        /// <summary>
        /// The Database ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Twitch Channel ID
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Twitch Channel Name
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// When the channel was added to the database
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// When the record was updated in the database.
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Twitch Userid of the person who created the channel.
        /// </summary>
        public string OwnerUserId { get; set; }

    }
}
