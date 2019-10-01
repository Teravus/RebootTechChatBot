using RebootTechBotLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Enums;

namespace RebootTechBotLib.Utility
{
    public static class Utilities
    {
        public static CommandPermissions GetCommandPermissionFromTwitchLibUserType(UserType chatUserType, int reputationpoints)
        {
            CommandPermissions resultPerms;
            switch (chatUserType)
            {
                case UserType.Viewer:
                    resultPerms = CommandPermissions.Unknown | CommandPermissions.Viewer;
                    break;
                case UserType.Admin:
                case UserType.Broadcaster:
                    resultPerms = CommandPermissions.Mod | CommandPermissions.Regular | CommandPermissions.Unknown | CommandPermissions.Viewer | CommandPermissions.BroadCaster;
                    break;
                case UserType.GlobalModerator:
                case UserType.Moderator:
                    resultPerms = CommandPermissions.Mod | CommandPermissions.Regular | CommandPermissions.Unknown | CommandPermissions.Viewer ;
                    break;
                case UserType.Staff:
                    resultPerms = CommandPermissions.Mod | CommandPermissions.Regular | CommandPermissions.Unknown | CommandPermissions.Viewer | CommandPermissions.BroadCaster;
                    break;
                default:
                    resultPerms = CommandPermissions.Unknown;
                    break;

            }
            if (reputationpoints > 30)
                resultPerms = resultPerms | CommandPermissions.Regular;

            return resultPerms;
        }

        // Used from OpenSimulator https://github.com/opensim/opensim/blob/master/OpenSim/Framework/Util.cs
        // BSD simulator
        /// <summary>
        /// Environment.TickCount is an int but it counts all 32 bits so it goes positive
        /// and negative every 24.9 days. This trims down TickCount so it doesn't wrap
        /// for the callers.
        /// This trims it to a 12 day interval so don't let your frame time get too long.
        /// </summary>
        /// <returns></returns>
        public static Int32 EnvironmentTickCount()
        {
            return Environment.TickCount & EnvironmentTickCountMask;
        }

        const Int32 EnvironmentTickCountMask = 0x3fffffff;

        /// <summary>
        /// Environment.TickCount is an int but it counts all 32 bits so it goes positive
        /// and negative every 24.9 days. Subtracts the passed value (previously fetched by
        /// 'EnvironmentTickCount()') and accounts for any wrapping.
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="prevValue"></param>
        /// <returns>subtraction of passed prevValue from current Environment.TickCount</returns>
        public static Int32 EnvironmentTickCountSubtract(Int32 newValue, Int32 prevValue)
        {
            Int32 diff = newValue - prevValue;
            return (diff >= 0) ? diff : (diff + EnvironmentTickCountMask + 1);
        }

        /// <summary>
        /// Environment.TickCount is an int but it counts all 32 bits so it goes positive
        /// and negative every 24.9 days. Subtracts the passed value (previously fetched by
        /// 'EnvironmentTickCount()') and accounts for any wrapping.
        /// </summary>
        /// <returns>subtraction of passed prevValue from current Environment.TickCount</returns>
        public static Int32 EnvironmentTickCountSubtract(Int32 prevValue)
        {
            return EnvironmentTickCountSubtract(EnvironmentTickCount(), prevValue);
        }

        // Returns value of Tick Count A - TickCount B accounting for wrapping of TickCount
        // Assumes both tcA and tcB came from previous calls to Util.EnvironmentTickCount().
        // A positive return value indicates A occured later than B
        public static Int32 EnvironmentTickCountCompare(Int32 tcA, Int32 tcB)
        {
            // A, B and TC are all between 0 and 0x3fffffff
            int tc = EnvironmentTickCount();

            if (tc - tcA >= 0)
                tcA += EnvironmentTickCountMask + 1;

            if (tc - tcB >= 0)
                tcB += EnvironmentTickCountMask + 1;

            return tcA - tcB;
        }

        public static bool MessageTriggeredSymbolCheck(string message, int maxNonAlpha, float maxPercent)
        {
            bool result = false;
            int length = message.Length;
            int totalNonAlphanumericCount = 1;
            for (int i=0; i< length; i++)
            {
                char c = message[i];
                if (!(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
                {
                    totalNonAlphanumericCount++;

                    if (totalNonAlphanumericCount >= maxNonAlpha || ((totalNonAlphanumericCount / length) >= maxPercent))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        public static bool hasLongNonAlphanumericSequence(string message, int maxLength)
        {
            bool hasLongRepeatingSequence = false;
            int totalRepeatingSequence = 1;

            for (int i = 0; i < message.Length; i++)
            {
                char c = message[i];

                if (!(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
                {
                    totalRepeatingSequence++;
                    if (totalRepeatingSequence >= maxLength)
                    {
                        hasLongRepeatingSequence = true;
                        break;
                    }
                }
                else
                {
                    totalRepeatingSequence = 1;
                }
            }

            return hasLongRepeatingSequence;
        }
        public static bool hasLongCharacterSequence(string message, int maxLength)
        {
            bool hasLongRepeatingSequence = false;
            int totalRepeatingSequence = 1;
            char lastCharacter = message[0];

            for (int i = 1; i < message.Length; i++)
            {
                char c = message[i];

                if (c == lastCharacter)
                {
                    totalRepeatingSequence++;
                    if (totalRepeatingSequence >= maxLength)
                    {
                        hasLongRepeatingSequence = true;
                        break;
                    }
                }
                else
                {
                    totalRepeatingSequence = 1;
                }
                lastCharacter = c;
            }

            return hasLongRepeatingSequence;
        }
        
        public static bool hasRepeatingWordsOrSequence(string message, int maxLength, int maxTimes, string rawEmoteIndexes)
        {
            bool hasLongRepeatingSequence = false;
            int totalRepeatingSequence = 1;
            Dictionary<string, int> repeats = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            String[] messageParts = getMessageWithoutEmotes(message, rawEmoteIndexes).Split(' ');
            String lastWord = messageParts[0];

            for (int i = 1; i < messageParts.Length; i++)
            {
                String word = messageParts[i];

                if (word.ToLowerInvariant() == (lastWord.ToLowerInvariant()))
                {
                    totalRepeatingSequence++;
                    if (totalRepeatingSequence >= maxLength)
                    {
                        hasLongRepeatingSequence = true;
                        break;
                    }
                }
                else
                {
                    totalRepeatingSequence = 1;
                }

                if (!repeats.ContainsKey(word))
                {
                    repeats.Add(word, 1);
                }
                else
                {
                    int times = (repeats[word] + 1);
                    // If we said it the max times, break and return.
                    if (times >= maxTimes)
                    {
                        hasLongRepeatingSequence = true;
                        break;
                    }
                    repeats[word] = times;
                }
                lastWord = word;
            }

            return hasLongRepeatingSequence;
        }
        public static bool hasMaximumCaps(string message, int maxCaps, float maxPercent, string rawEmoteIndexes)
        {
            // Remove Twitch emotes from the message.
            message = getMessageWithoutEmotes(message, rawEmoteIndexes);

            bool hasMaxCaps = false;
            float messageLength = message.Length;
            int totalCapsCount = 1;

            for (int i = 0; i < message.Length; i++)
            {
                char c = message[i];

                if ((c==char.ToUpperInvariant(c)))
                {
                    totalCapsCount++;
                    if (totalCapsCount >= maxCaps || ((totalCapsCount / messageLength) >= maxPercent))
                    {
                        hasMaxCaps = true;
                        break;
                    }
                }
            }

            return hasMaxCaps;
        }


        // winterbot utils
        public static bool HasSpecialCharacter(string str)
        {
            for (int i = 0; i < str.Length; ++i)
                if (!Allowed(str[i]))
                    return true;

            return false;
        }

        public static bool Allowed(char c)
        {
            if (c < 255)
                return true;

            // punctuation block
            if (0x2010 <= c && c <= 0x2049)
                return true;

            return c == '™' || c == '♥' || c == '…' || c == '€'; //(IsKoreanCharacter(c))
        }

        public static bool IsKoreanCharacter(char c)
        {
            return (0xac00 <= c && c <= 0xd7af) ||
                (0x1100 <= c && c <= 0x11ff) ||
                (0x3130 <= c && c <= 0x318f) ||
                (0x3200 <= c && c <= 0x32ff) ||
                (0xa960 <= c && c <= 0xa97f) ||
                (0xd7b0 <= c && c <= 0xd7ff) ||
                (0xff00 <= c && c <= 0xffef);
        }
        // winter bot utils end.
        // tools from //
        //https://github.com/Pdbz199/TwitchSpamBlocker/blob/master/bot.js 

        private static string findSmallestRepeat(string message)
        {
            string returnMessage = string.Empty;
            for (int i=0; i<message.Length; i++)
            {

                returnMessage = message.Substring(0, i + 1);
                if (returnMessage == message.Substring(message.Length - returnMessage.Length)) break;

            }
            return returnMessage;
        }
        private static Dictionary<int, int> getEmotesIndexMap(String rawEmoteIndexes)
        {
            Dictionary<int, int> emoteIndexMap = new Dictionary<int, int>();
            string[] indexArray;

            if (rawEmoteIndexes.Length > 0)
            {
                indexArray = rawEmoteIndexes.Split('/');

                for (int i = 0; i < indexArray.Length; i++)
                {
                    String[] indexes = indexArray[i].Substring(indexArray[i].IndexOf(":") + 1).Split(',');
                    for (int j = 0; j < indexes.Length; j++)
                    {
                        String[] index = indexes[j].Split('-');

                        emoteIndexMap.Add(Int32.Parse(index[0]), Int32.Parse(index[1]));
                    }
                }
            }

            return emoteIndexMap;
        }
        private static string getMessageWithoutEmotes(string message, String rawEmoteIndexes)
        {
            Dictionary<int, int> emoteIndexMap = getEmotesIndexMap(rawEmoteIndexes);

            for (int i = message.Length - 1; i >= 0; i--)
            {
                if (emoteIndexMap.ContainsKey(i))
                {
                    message = message.Substring(0, i) + message.Substring(emoteIndexMap[i] + 1);
                }
            }

            return message.Trim();
        }
    }
}
