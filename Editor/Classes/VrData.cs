using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

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
        
        // Getting VR data from server for defined applicationIdentifier, organisationCode and activityId
        public IEnumerator GetVrData(string serverUrl, string applicationIdentifier, string organisationCode, string activityId, string environmentId, Action<VrData> responseCallback)
        {
            var url = serverUrl + "/public/vr-data/" + applicationIdentifier + "/" + organisationCode + "/" +
                      activityId;

            if (!string.IsNullOrEmpty(environmentId))
            {
                url += "/" + environmentId;
            }

            Debug.Log("Getting data from: " + url);
            UnityWebRequest request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                var json = request.downloadHandler.text;
                // Transform JSON text to VR data object
                var data = CreateFromJson(json);

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                responseCallback.Invoke(data);
            }
        }
        
        private VrData CreateFromJson(string json)
        {
            return JsonUtility.FromJson<VrData>(json);
        }

        public override string ToString()
        {
            return string.Format(
                "[VrData: application_identifier={0}, log_version={1}, start={2}, end={3}, log_rate={4}, custom_data={5}]",
                application_identifier, log_version, start, end, log_rate, custom_data ?? "null");
        }
    }
}
