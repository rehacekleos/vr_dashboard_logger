using System;
using System.Collections;
using System.Net;
using System.Text;
using Editor.Classes;
using UnityEngine;
using UnityEngine.Networking;

namespace Editor
{
    public class LoggerHelper
    {


        public IEnumerator SendActivity(Activity activity, Action<bool> responseCallback)
        {
            var url = "";
            var activityBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(activity));

            UnityWebRequest wr = new UnityWebRequest(url, WebRequestMethods.Http.Post);
            UploadHandler uploader = new UploadHandlerRaw(activityBytes);

            uploader.contentType = "application/json";

            wr.uploadHandler = uploader;

            yield return wr.SendWebRequest();

            responseCallback.Invoke(wr.result == UnityWebRequest.Result.Success);
        }
    
    }
}
