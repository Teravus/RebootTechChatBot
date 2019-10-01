using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBotLib.Command
{

    public delegate void CommandDelegate(string module, string[] cmd);

    public interface ICommands
    {
        /// <summary>
        /// Get help for the given help string
        /// </summary>
        /// <param name="cmd">The given help string</param>
        /// <returns></returns>
        List<string> GetHelp(string[] cmd);

        /// <summary>
        /// Add a command to those which can be invoked from chat
        /// </summary>
        /// <param name="module"></param>
        /// <param name="shared"></param>
        /// <param name="command"></param>
        /// <param name="help"></param>
        /// <param name="longhelp"></param>
        /// <param name="fn"></param>
        void AddCommand(string module, bool shared, string command, string help, string longhelp, CommandDelegate fn, int cooldownms = 0);
        void RemCommand(string module, bool shared, string command, string help, string longhelp, CommandDelegate fn);
        /// <summary>
        /// Has the given command already been registered
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        bool HasCommand(string command);

        string[] FindNextOption(string[] command, bool term);

        string[] Resolve(string[] command);


    }

    public interface ICommandProcessor
    {
        ICommands Commands { get; }

        void RunCommand(string cmd);

    }
}
