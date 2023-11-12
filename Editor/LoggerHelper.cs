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


        public IEnumerator SendActivity(Activity activity)
        {
            var url = "";
            var activityBytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(activity));

            UnityWebRequest wr = new UnityWebRequest(url, WebRequestMethods.Http.Post);
            UploadHandler uploader = new UploadHandlerRaw(activityBytes);

            uploader.contentType = "application/json";

            wr.uploadHandler = uploader;

            yield return wr.SendWebRequest();

            if (wr.result != UnityWebRequest.Result.Success)
            {
                yield return false;
            }
            else
            {
                yield return true;
            }
        }
    
    }
}
