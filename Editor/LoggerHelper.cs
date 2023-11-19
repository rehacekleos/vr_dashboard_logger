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

            Debug.Log("Getting data from: " + url);
            UnityWebRequest request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                responseCallback.Invoke(null);
            }
            else
            {
                var json = request.downloadHandler.text;
                // Transform JSON text to VR data object
                var data = CreateVrDataFromJson(json);

                if (data == null)
                {
                    responseCallback.Invoke(null);
                    throw new ArgumentNullException(nameof(data));
                }

                responseCallback.Invoke(data);
            }
        }


        public IEnumerator SendActivity(string apiBaseUrl, Activity activity, Action<bool> responseCallback)
        {
            var url = apiBaseUrl + "/";
            var activityBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(activity));

            UnityWebRequest wr = new UnityWebRequest(url, WebRequestMethods.Http.Post);
            UploadHandler uploader = new UploadHandlerRaw(activityBytes);

            uploader.contentType = "application/json";

            wr.uploadHandler = uploader;

            yield return wr.SendWebRequest();

            responseCallback.Invoke(wr.result == UnityWebRequest.Result.Success);
        }

        public IEnumerator GetParticipants(string apiBaseUrl, string applicationIdentifier, string organisationCode, Action<List<Participant>> responseCallback)
        {
            var url = apiBaseUrl + "/public/participants/" + applicationIdentifier + "/" + organisationCode;
            UnityWebRequest request = UnityWebRequest.Get(url);
            
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
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
        
        private VrData CreateVrDataFromJson(string json)
        {
            return JsonConvert.DeserializeObject<VrData>(json);
        }
    }
}
