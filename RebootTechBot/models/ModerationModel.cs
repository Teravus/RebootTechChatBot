using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTechBot.models
{
    public class ModerationModel
    {
        public string SelectedModerationPreset { get; set; }

        public bool ModerationChanged { get; set; }

        public bool CapsModeration { get; set; }
        public float CapsModerationPercent { get; set; }
        public int CapsModerationTriggerMinLength { get; set; }

        public bool SymbolModeration { get; set; }
        public float SymbolModerationPercent { get; set; }
        public int SymbolModerationTriggerMinLength { get; set; }
        public int SymbolModerationMaxSymbolGrouping { get; set; }

        public bool MessageLengthModeration { get; set; }
        public int MessageLengthModerationMaxLength { get; set; }

        public bool SpamModeration { get; set; }
        public int SpamModerationNumRepeatedMessages { get; set; }
        public int SpamModerationSeconds { get; set; }
        public bool SpamModerationAcrossAllUsers { get; set; }

        public bool EmotesModeration { get; set; }
        public float EmotesModerationPercentCaps { get; set; }
        public int EmotesModerationMinLength { get; set; }

        public bool ColorModeration { get; set; }

        public bool ModerationExceptions { get; set; }
        public bool ModerationExceptionsViewer { get; set; }
        public bool ModerationExceptionsFollower { get; set; }
        public bool ModerationExceptionsRegular { get; set; }
        public bool ModerationExceptionsVIP { get; set; }
        public bool ModerationExceptionsSubscribers { get; set; }
        public bool ModerationExceptionsMods { get; set; }
        public int ModerationExceptionsViewerSeconds { get; set; }
        public int ModerationExceptionsFollowerHours { get; set; }
        public string ModerationAction1stOffense { get; set; }
        public string ModerationAction2ndOffense { get; set; }
        public string ModerationAction3rdOffense { get; set; }
        public List<ModerationBannedWord> BannedWord { get; set; }

    }
}
