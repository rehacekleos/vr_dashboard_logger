using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Editor.Classes;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Editor
{
    public class LoggerHelper
    {
        
        // Getting VR data from server for defined applicationIdentifier, organisationCode and activityId
        public IEnumerator GetVrData(string serverUrl, string applicationIdentifier, string organisationCode, string activityId, string environmentId, Action<VrData> responseCallback)
        {
            var url = serverUrl + "/public/vr-data/" + applicationIdentifier + "/" + organisationCode + "/" +
                      activityId;

            if (!string.IsNullOrEmpty(environmentId))
            {
                url += "/" + environmentId;
            }

            Debug.Log("[Vr Logger] Getting data from: " + url);
            UnityWebRequest request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[Vr Logger] " + request.error + "with error message: " + request.downloadHandler.text);
                responseCallback.Invoke(null);
            }
            else
            {
                var json = request.downloadHandler.text;
                // Transform JSON text to VR data object
                var data = JsonConvert.DeserializeObject<VrData>(json);

                if (data == null)
                {
                    responseCallback.Invoke(null);
                    throw new ArgumentNullException(nameof(data));
                }

                responseCallback.Invoke(data);
            }
        }

        public void SaveActivityIntoFile(Activity activity)
        {
            string json = JsonConvert.SerializeObject(activity);
            var now = DateTime.Now.ToString("yyyy-MM-dd\\THH:mm:ss\\Z");
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/vr-dashboard");
            var filePath = Application.persistentDataPath + "/vr-dashboard/" + now + ".json";
            Debug.Log("[Vr Logger] Saving Activity into file with path: " + filePath);
            System.IO.File.WriteAllText(filePath, json);
        }

        public IEnumerator SendActivity(string apiBaseUrl, string applicationIdentifier, Activity activity,
            Action<bool> responseCallback)
        {
            var url = apiBaseUrl + "/public/activity/" + applicationIdentifier;
            var activityBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(activity));

            UnityWebRequest wr = new UnityWebRequest(url, WebRequestMethods.Http.Post);
            UploadHandler uploader = new UploadHandlerRaw(activityBytes);

            uploader.contentType = "application/json";

            wr.uploadHandler = uploader;

            yield return wr.SendWebRequest();

            responseCallback.Invoke(wr.result == UnityWebRequest.Result.Success);
            Debug.Log("[Vr Logger] Activity sent.");
        }

        public IEnumerator GetParticipants(string apiBaseUrl, string applicationIdentifier, string organisationCode, Action<List<Participant>> responseCallback)
        {
            var url = apiBaseUrl + "/public/participants/" + applicationIdentifier + "/" + organisationCode;
            UnityWebRequest request = UnityWebRequest.Get(url);
            
            Debug.Log("[Vr Logger] Getting Participant from server: " + url);
            
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[Vr Logger]" + request.error + "with error message: " + request.downloadHandler.text);
                responseCallback.Invoke(null);
            }
            else
            {
                var json = request.downloadHandler.text;
                // Transform JSON text to VR data object
                var data = JsonConvert.DeserializeObject<ParticipantList>(json);

                if (data == null)
                {
                    responseCallback.Invoke(null);
                    throw new ArgumentNullException(nameof(data));
                }

                responseCallback.Invoke(data.participants);
            }
        }
    }
}
