using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Editor.Classes
{
    [Serializable]
    public class VrData
    {
        public string application_identifier;
        public string log_version;
        public DateTime start;
        public DateTime end;
        public int log_rate;
        public List<Record> records;
        [CanBeNull] public string custom_data;

        public VrData(string applicationIdentifier, string logVersion, int logRate, [CanBeNull] string customData)
        {
            application_identifier = applicationIdentifier;
            log_version = logVersion;
            log_rate = logRate;
            records = new List<Record>();
            custom_data = customData;
        }
    }
}
