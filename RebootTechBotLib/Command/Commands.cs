// New BSD/MIT licensed

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RebootTechBotLib.Command
{
    public class Commands : ICommands
    {
        /// <summary>
        /// Encapsulation of a command that can be invoked from the console
        /// </summary>
        private class CommandInfo
        {
            public string module;

            public bool shared;
            public string help_text;
            public string long_help;
            public int cool_down;
            public int ticks_since_last_use;
           
            public List<CommandDelegate> fn;
        }

        private Dictionary<string, List<CommandInfo>> m_modulesCommands = new Dictionary<string, List<CommandInfo>>();//StringComparer.OrdinalIgnoreCase
        private Dictionary<string, object> tree = new Dictionary<string, object>();//StringComparer.OrdinalIgnoreCase

        public List<string> GetHelp(string[] cmd)
        {
            List<string> help = new List<string>();
            List<string> helpParts = new List<string>(cmd);

            // remove initial keyword.  This is always 'help'
            // <strikethrough>help</strikethrough> command
            helpParts.RemoveAt(0);
            help.Add(""); // will become a newline
            if (helpParts.Count == 0)
            {
                help.Add(QuoteGeneralHelpText);
                help.Add(ItemHelpText);
                help.AddRange(CollectModuleHelp(tree));
            }
            else if (helpParts.Count == 1 && helpParts[0] == "all")
            {
                help.AddRange(CollectAllCommandsHelp());
            }
            else
            {
                help.AddRange(CollectHelp(helpParts));
            }
            help.Add("");
            return help;

        }

        public const string QuoteGeneralHelpText = "To enter an argument that contains spaces, surround the argument with double quotes.";
        public const string ItemHelpText = @"For more information, type 'help all' to get a list of all commands, or type help <item> where <item> is one of the following:";

        private List<string> CollectHelp (List<string> helpParts)
        {
            string originalHelpRequest = string.Join(" ", helpParts.ToArray());

            List<string> help = new List<string>();
            if (TryCollectModuleHelp(originalHelpRequest, help))
            {
                help.Insert(0, ItemHelpText);
                return help;
            }
            Dictionary<string, object> current = tree;
            while(helpParts.Count > 0)
            {
                string helpPart = helpParts[0];

                if (!current.ContainsKey(helpPart))
                {
                    break;
                }
                if (current[helpPart] is Dictionary<string,object>)
                {
                    current = (Dictionary<string, object>)current[helpPart];
                }
                helpParts.RemoveAt(0);
                
            }
            if (current.ContainsKey(string.Empty))
            {
                CommandInfo commandInfo = (CommandInfo)current[string.Empty];
                help.Add(commandInfo.help_text);
                help.Add(commandInfo.long_help);
            }
            else
            {
                help.Add(string.Format("No help is available for {0}", originalHelpRequest));
            }
            return help;
        }
        private List<string> CollectAllCommandsHelp()
        {
            List<string> help = new List<string>();
            lock(m_modulesCommands)
            {
                foreach(List<CommandInfo> commands in m_modulesCommands.Values)
                {
                    List<string> ourHelpText = commands.ConvertAll(c => string.Format("{0} - {1}", c.help_text, c.long_help));
                    help.AddRange(ourHelpText);
                }
            }
            help.Sort();
            return help;
        }
        private bool TryCollectModuleHelp(string moduleName, List<string> helpText)
        {
            return TryCollectHelp(moduleName, helpText);
            //lock (m_modulesCommands)
            //{
            //    foreach (string key in m_modulesCommands.Keys)
            //    {
            //        if (moduleName.ToLowerInvariant() == key.ToLowerInvariant())
            //        {
            //            List<CommandInfo> commands = m_modulesCommands[key];
            //            var ourHelpText = commands.ConvertAll(c => string.Format("{0} - {1}", c.help_text, c.long_help));
            //            ourHelpText.Sort();
            //            helpText.AddRange(ourHelpText);
            //            return true;
            //        }
            //    }
            //    return false;
            //}
        }

        private bool TryCollectHelp (string moduleName, List<string> helpText)
        {
            lock (m_modulesCommands)
            {
                foreach (string key in m_modulesCommands.Keys)
                {
                    if (moduleName.ToLowerInvariant() == key.ToLowerInvariant())
                    {
                        List<CommandInfo> commands = m_modulesCommands[key];
                        var ourHelpText = commands.ConvertAll(c => string.Format("{0} - {1}", c.help_text, c.long_help));
                        ourHelpText.Sort();
                        helpText.AddRange(ourHelpText);
                        return true;
                    }
                }
                return false;
            }
        }
        private List<string> CollectModuleHelp (Dictionary<string, object> dict)
        {
            lock (m_modulesCommands)
            {
                List<string> helpText = new List<string>(m_modulesCommands.Keys);
                helpText.Sort();
                return helpText;
            }

        }

        public void AddCommand(string module, bool shared, string command, string help, string longhelp, CommandDelegate fn, int cooldownms = 0)
        {
            string[] parts = Parse(command);

            Dictionary<string, object> current = tree;

            foreach (var part in parts)
            {
                if (current.ContainsKey(part))
                {
                    if (current[part] is Dictionary<string, object>)
                        current = (Dictionary<string, object>)current[part];
                    else
                        return;
                }
                else
                {
                    current[part] = new Dictionary<string, object>();
                    current = (Dictionary<string, object>)current[part];

                }
            }

            CommandInfo info = null;

            if (current.ContainsKey(string.Empty))
            {
                info = (CommandInfo)current[string.Empty];
                if (!info.shared && !info.fn.Contains(fn))
                    info.fn.Add(fn);
                return;
            }

            info = new CommandInfo()
            {
                module = module,
                shared = shared,
                help_text = help,
                long_help = longhelp,
                cool_down = cooldownms,
                fn = new List<CommandDelegate>() { fn }

            };
            current[string.Empty] = info;

            lock (m_modulesCommands)
            {
                List<CommandInfo> commands;
                if (m_modulesCommands.ContainsKey(module))
                {
                    commands = m_modulesCommands[module];
                }
                else
                {
                    commands = new List<CommandInfo>();
                    m_modulesCommands[module] = commands;
                }

                commands.Add(info);
            }
        }
        public void RemCommand(string module, bool shared, string command, string help, string longhelp, CommandDelegate fn)
        {
            string[] result = null;
            var resolveresult = ResolveCommand(Parse(command), out result);
            m_modulesCommands.Remove(command);//resolveresult
            tree.Remove(command);

        }
        public bool HasCommand(string command)
        {
            string[] result = null;
            return ResolveCommand(Parse(command), out result) != null;
        }

        public string[] FindNextOption(string[] cmd, bool term)
        {
            Dictionary<string, object> current = tree;
            int remaining = cmd.Length;
            foreach (var s in cmd)
            {
                --remaining;
                List<string> found = new List<string>();
                foreach (string opt in current.Keys)
                {
                    if (remaining > 0 && opt == s)
                    {
                        found.Clear();
                        found.Add(opt);
                        break;
                    }
                    //if (opt.StartsWith(s))
                    //{
                    //    found.Add(opt);
                    //}
                }
                if (found.Count == 1 && (remaining != 0 || term))
                {
                    current = (Dictionary<string, object>)current[found[0]];
                }
                else if (found.Count > 0)
                {
                    return found.ToArray();
                }
                else
                    break;
                    
            }
            if (current.Count > 1)
            {
                List<string> choices = new List<string>();
                bool addcr = false;
                foreach (string s in current.Keys)
                {
                    if (s == string.Empty)
                    {
                        CommandInfo ci = (CommandInfo)current[string.Empty];
                        if (ci.fn.Count != 0)
                        {
                            addcr = true;
                        }
                        else
                            choices.Add(s);
                    }
                    if (addcr)
                        choices.Add("<cr>");// placeholder
                    return choices.ToArray();
                }
                
            }
            if (current.ContainsKey(string.Empty))
                return new string[] { "Command help: " + ((CommandInfo)current[string.Empty]).help_text };

            return new string[] { new List<string>(current.Keys)[0] };
        }

        public string[] Resolve(string[] cmd)
        {
            string[] result = null;
            CommandInfo ci = ResolveCommand(cmd, out result);

            if (ci == null)
            {
                return new string[0];
            }
            if (ci.fn.Count == 0)
            {
                return new string[0];
            }
            foreach (CommandDelegate fn in ci.fn)
            {
                if (fn != null)
                {
                    int currentime = Utility.Utilities.EnvironmentTickCount();
                    if (ci.cool_down == 0 || Utility.Utilities.EnvironmentTickCountSubtract(currentime, ci.ticks_since_last_use) > ci.cool_down)
                    {
                        ci.ticks_since_last_use = Utility.Utilities.EnvironmentTickCount();
                        fn(ci.module, result);
                    }

                }
                else
                    return new string[0];

            }
            return result;

        }

        private CommandInfo ResolveCommand( string[] cmd, out string[] result)
        {
            result = cmd;
            Dictionary<string, object> current = tree;
            int index = -1;
            foreach (string s in cmd)
            {
                ++index;
                List<string> found = new List<string>();
                try
                {
                    foreach (string opt in current.Keys)
                    {
                        if (opt == s)
                        {
                            found.Clear();
                            found.Add(opt);
                            break;
                        }
                        //if (opt.StartsWith(s))
                        //{
                        //    found.Add(opt);
                        //}
                    }
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
                if (found.Count == 1)
                {
                    result[index] = found[0];
                    current = (Dictionary<string, object>)current[found[0]];
                }
                else if (found.Count > 0)
                {
                    return null;
                }
                else
                {
                    break;
                }
            }
            if (current.ContainsKey(string.Empty))
                return (CommandInfo)current[string.Empty];

            return null;

        }

        private static Regex OptionRegex = new Regex("^--[a-zA-Z0-9-]+=$", RegexOptions.Compiled);

        public static string[] Parse(string text)
        {
            
            List<string> result = new List<string>();
            // Quoted segments
            string[] unquoted = text.Split(new char[] { '"' });

            // Loop over quoted segments with no quotes
            for (int index = 0; index < unquoted.Length; index++)
            {
                // We have an even number
                if (index % 2 == 0)
                {
                    bool option = false;
                    // split by spaces
                    string[] words = unquoted[index].Split(new char[] { ' ' });


                    foreach (var w in words)
                    {
                        // Exclude whitespace and null and empty string
                        if (!string.IsNullOrEmpty(w) && !string.IsNullOrWhiteSpace(w))
                        {
                            // check for quotes using our regular expression
                            if (OptionRegex.Match(w) == Match.Empty)
                                option = false;  // No quotes
                            else
                                option = true; // quotes;
                                
                            // save result
                            result.Add(w);
                        }
                    }
                    // if there was quotes, we have to put the quotes back
                    if (option)
                    {
                        if (index < (unquoted.Length - 1))
                        {
                            string optionText = result[result.Count - 1];

                            // remove improper result
                            result.RemoveAt(result.Count - 1);

                            // requote the result
                            optionText += "\"" + unquoted[index + 1] + "\"";

                            // add better quote
                            result.Add(optionText);

                            index++;
                        }
                    }
                }
                else
                {
                    result.Add(unquoted[index]);
                }
            }

            return result.ToArray();
        }
    }
}
