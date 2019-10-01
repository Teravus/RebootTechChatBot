using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Infrastructure
{
    public interface IRequestHandler
    {
        string ContentType { get; }

        string HttpMethod { get; }

        string Path { get; }
    }

    public interface IGenericHTTPHandler : IRequestHandler
    {
        Hashtable Handle(string path, Hashtable request);
    }
}
