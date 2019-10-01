using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Infrastructure
{
    public class MimeType
    {
        public string Extension { get; set; }
        public string Description { get; set; }
        public string MIME { get; set; }
        public bool binary { get; set; }
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}
