using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib
{
    public class Enums
    {
        public enum UserType : int
        {
            Viewer = 0,
            Moderator = 1,
            GlobalModerator = 2,
            Broadcaster = 3, 
            Admin = 4,
            Staff = 5
        }
    }
}
