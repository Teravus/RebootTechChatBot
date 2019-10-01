using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.SharedTypes
{
    public class SharedInformationalChatCommand
    {
        public int CommandId { get; set; }
        public string ChannelName { get; set; }
        public string CommandTrigger { get; set; }
        public string CommandResponse { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DateCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DateModified { get; set; }
        public string CommandPermissionRequired { get; set; }
        public int CoolDownSeconds { get; set; } = 0;
        public int IsActive { get; set; }
    }
}
