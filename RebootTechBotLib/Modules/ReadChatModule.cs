using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using RebootTechBotLib.Config;
using RebootTechBotLib.Infrastructure;
using System.Text.RegularExpressions;

namespace RebootTechBotLib.Modules
{
    public class ReadChatModule : IChatModule, IDisposable
    {
        private SpeechSynthesizer _tts;
        BotOutput m_output;
        private Queue<string> textbacklog = new Queue<string>();
        private int m_BackLogLengthMax = 10;
        private static readonly Regex wordPattern = new Regex(@"\w+", RegexOptions.Compiled);
        private HashSet<string> StopWords = new HashSet<string>() {"ourselves", "hers", "between", "yourself", "but", "again", "there", "about", "once", "during", "out", "very", "having", "with", "they", "own", "an", "be", "some", "for", "do", "its", "yours", "such", "into", "of", "most", "itself", "other", "off", "is", "s", "am", "or", "who", "as", "from", "him", "each", "the", "themselves", "until", "below", "are", "we", "these", "your", "his", "through", "don", "nor", "me", "were", "her", "more", "himself", "this", "down", "should", "our", "their", "while", "above", "both", "up", "to", "ours", "had", "she", "all", "no", "when", "at", "any", "before", "them", "same", "and", "been", "have", "in", "will", "on", "does", "yourselves", "then", "that", "because", "what", "over", "why", "so", "can", "did", "not", "now", "under", "he", "you", "herself", "has", "just", "where", "too", "only", "myself", "which", "those", "i", "after", "few", "whom", "t", "being", "if", "theirs", "my", "against", "a", "by", "doing", "it", "how", "further", "was", "here", "than"  };

        public ReadChatModule()
        {
            m_output = BotOutput.Instance;
        
                
        }

        public string Name => "ReadChat Module";

        public bool IsShared => true;

        public void ChannelJoined(TwitchChannel channel)
        {
            //m_output.ChatMessage(channel.Channel, string.Format("Hi denizens of my favorite channel {0}!", channel.Channel));
            channel.OnChannelChatMessage += Channel_OnChatMessage;
        }

        public void ChannelParted(TwitchChannel channel)
        {
           
            channel.OnChannelChatMessage -= Channel_OnChatMessage;
        }

        private void Channel_OnChatMessage(RTChatMessage message)
        {
            string dememestring = DeMemeString(message.Message);
            if (!string.IsNullOrEmpty(dememestring))
            {
                lock (textbacklog)
                    textbacklog.Enqueue(dememestring);


                _tts.SpeakAsync(dememestring);
            }
        }

        public void Initialize(BotConfig config)
        {
            m_output.LogMessage("info", string.Format("[READCHATMODULE]: I was Initialized! My name is {0}", config.general.BotName));
            SetUpSpeechSynth();
        }

        private string DeMemeString( string input)
        {
            string unmangledinput = input;
            string dememedstring = unmangledinput;
            Dictionary<string, int> wordcounts = countWordsInString(input);

            List<KeyValuePair<string,int>> orderedByCountvals = wordcounts.Where(y=> y.Value > 2).OrderBy(f => f.Value).ToList();
            foreach(var orderedval in orderedByCountvals)
            {
                if (!StopWords.Contains(orderedval.Key.ToLowerInvariant()))
                    dememedstring = dememedstring.Replace(orderedval.Key, "");
            }
            List<KeyValuePair<string, int>> orderedByLengthvals = wordcounts.Where(y => y.Key.Length > 5).ToList();

            foreach (var orderedval in orderedByLengthvals)
            {
                if (countSyllables(orderedval.Key) > 10)
                    dememedstring = dememedstring.Replace(orderedval.Key, "");
            }



            return dememedstring;
        }
        private static int countSyllables(string word)
        {
            //The letter 'y' can be counted as a vowel, only if it
            //creates the sound of a vowel (a, e, i, o, u).
            char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'y' };
            char[] currentWord = word.ToCharArray();
            int numVowels = 0;
            bool lastWasVowel = false;
            foreach (char wc in currentWord)
            {
                bool foundVowel = false;
                foreach (char v in vowels)
                {
                    //don't count diphthongs
                    if ((v == wc) && lastWasVowel)
                    {
                        foundVowel = true;
                        lastWasVowel = true;
                        break;
                    }
                    else if (v == wc && !lastWasVowel)
                    {
                        numVowels++;
                        foundVowel = true;
                        lastWasVowel = true;
                        break;
                    }
                }
                // If full cycle and no vowel found, set lastWasVowel to false;
                if (!foundVowel)
                    lastWasVowel = false;
            }
            // Remove es, it's _usually? silent
            if (word.Length > 2 &&
                    word.Substring(word.Length - 2) == "es")
                numVowels--;
            // remove silent e
            else if (word.Length > 1 &&
                    word.Substring(word.Length - 1) == "e")
                numVowels--;
            return numVowels;
        }
        private Dictionary<string, int> countWordsInString(string str)
        {
            var content = str;
            
             Dictionary<string, int> words = new Dictionary<string, int>();

            int currentCount = 0;
            foreach (Match match in wordPattern.Matches(content))
            {
                currentCount = 0;
                words.TryGetValue(match.Value, out currentCount);

                currentCount++;
                words[match.Value] = currentCount;
            }
            return words;
        }
        public void Started()
        {

        }
        public void Shutdown()
        {
            m_output.LogMessage("info", "NOOOOOOOOOOOOOOOOoooooooooooooooooooooooooooooooooooooo............................... monkaS");
            lock (textbacklog)
                textbacklog.Clear();
        }
        private void SetUpSpeechSynth()
        {
            _tts = new SpeechSynthesizer();
            var voices = _tts.GetInstalledVoices();
            _tts.SetOutputToDefaultAudioDevice();
            //_tts.SelectVoice("Microsoft David Desktop");
            _tts.SpeakCompleted += _tts_SpeakCompleted;
        }
        private void ShutdownSpeechSynth()
        {
            _tts.SpeakCompleted -= _tts_SpeakCompleted;
        }

        private void _tts_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            lock (textbacklog)
            {
                if (textbacklog.Count > 0)
                    textbacklog.Dequeue();
                if (textbacklog.Count > m_BackLogLengthMax)
                    _tts.SpeakAsyncCancelAll();
            }
            //throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _tts.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ReadChatModule() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
