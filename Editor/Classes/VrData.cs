using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Object = System.Object;

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
        [CanBeNull] public Object custom_data;

        public VrData()
        {
        }

        public VrData(string applicationIdentifier, string logVersion, int logRate)
        {
            application_identifier = applicationIdentifier;
            log_version = logVersion;
            log_rate = logRate;
            records = new List<Record>();
        }

        public override string ToString()
        {
            return string.Format(
                "[VrData: application_identifier={0}, log_version={1}, start={2}, end={3}, log_rate={4}, custom_data={5}]",
                application_identifier, log_version, start, end, log_rate, custom_data ?? "null");
        }
    }
}
