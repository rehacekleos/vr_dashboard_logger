using System;
using JetBrains.Annotations;

namespace VrDashboardLogger.Editor.Classes
{
    [Serializable]
    public class Activity
    {
        public VrData data;
        public bool anonymous;
        public string organisation_code;
        [CanBeNull] public string participantId;

        public Activity(VrData data, bool anonymous, string organisationCode, [CanBeNull] string participantId)
        {
            this.data = data;
            this.anonymous = anonymous;
            organisation_code = organisationCode;
            this.participantId = participantId;
        }
    }
}
