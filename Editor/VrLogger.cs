using System;
using System.Collections;
using System.Collections.Generic;
using Editor.Classes;
using Newtonsoft.Json;
using UnityEngine;
using Object = System.Object;

namespace Editor
{
    public class VrLogger : LoggerBase
    {
        private readonly WebGLHelper _webGLHelper = new WebGLHelper();
        public string apiBaseUrl;
        public string applicationIdentifier;
        public int logRate = 300;
        public string logVersion;
        
        private string _organisationCode;
        private string _participantId;
        private bool _isAnonymous = true;
        private Object _customData;
        private bool _logging;

        
        /// <summary>
        /// This method is for initialize logger.
        /// <br></br>
        /// Need to be call after SetOrganisation(), SetParticipant() and before StartLogging().
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <seealso cref="SetOrganisation"/>
        /// <seealso cref="SetParticipant"/>
        /// <seealso cref="StartLogging"/>
        public void InitializeLogger()
        {
            if (_organisationCode == null)
            {
                throw new Exception("Organisation is not set!");
            }

            string participant = null;
            if (!string.IsNullOrEmpty(_participantId))
            {
                participant = _participantId;
            }

            var vrData = new VrData(applicationIdentifier, logVersion, logRate);
            Activity = new Activity(vrData, _isAnonymous, _organisationCode, participant);
            
            Debug.Log("[Vr Logger] Initialized.");
        }

        
        /// <summary>
        /// Starting logging VR data.
        /// <br></br>
        /// Need to be call after InitializeLogger().
        /// </summary>
        /// <param name="environment">Environmental key</param>
        /// <exception cref="Exception">Logging is active!</exception>
        /// <exception cref="Exception">Logger is not Initialized!</exception>
        /// <seealso cref="InitializeLogger"/>
        public void StartLogging(string environment)
        {
            if (_logging)
            {
                throw new Exception("[Vr Logger] Logging is active!");
            }
            
            if (Activity == null)
            {
                throw new Exception("[Vr Logger] Logger is not Initialized!");
            }

            _logging = true;
            Environment = environment;
            Activity.data.start = DateTime.Now;

            var logRateInSeconds = logRate / 1000f;

            StartCoroutine(LoggingCoroutine(logRateInSeconds));
            Debug.Log("[Vr Logger] Logging started.");
        }
        
        
        /// <summary>
        /// Stop logging VR data.
        /// </summary>
        /// <exception cref="Exception">Logging is not active!</exception>
        public void StopLogging()
        {
            if (_logging == false)
            {
                throw new Exception("[Vr Logger] Logging is not active!");
            }
            
            Activity.data.end = DateTime.Now;
            
            StopCoroutine(LoggingCoroutine(0));
            _logging = false;
            Debug.Log("[Vr Logger] Logging stopped.");
        }

        /// <summary>
        /// Sending logged activity to server.
        /// <br></br>
        /// Callback return true if success else return false.
        /// </summary>
        /// <param name="responseCallback">Function which accept boolean.</param>
        /// <param name="savetoLocalFile">If true then save activity into local file</param>
        public void SendActivity(Action<bool> responseCallback, bool savetoLocalFile = false)
        {
            
            Activity.data.custom_data = _customData;
            
            if (savetoLocalFile)
            {
                LoggerHelper.SaveActivityIntoFile(Activity);
            }
            
            StartCoroutine(LoggerHelper.SendActivity(apiBaseUrl, applicationIdentifier, Activity, responseCallback));
        }

        /// <summary>
        /// Getting participants information for Organisation and Application.
        /// <br></br>
        /// Must be call after InitializeLogger() and SetOrganisation()
        /// <br></br>
        /// Callback return List of Participant. If error occured than return null.
        /// </summary>
        /// <param name="responseCallback">Function which accept List of Participants.</param>
        /// <seealso cref="InitializeLogger"/>
        /// <seealso cref="SetOrganisation"/>
        public void GetParticipants(Action<List<Participant>> responseCallback)
        {
            StartCoroutine(LoggerHelper.GetParticipants(apiBaseUrl, applicationIdentifier, _organisationCode, responseCallback));
        }


        /// <summary>
        /// Getting VrData. Using for WebGL module.
        /// </summary>
        /// <param name="responseCallback">Returning VR data get from server</param>
        /// <param name="customServerUrl">
        /// (Optional) Custom Server URL. Standard resolve serverUrl from hosting server.
        /// For example: http://127.0.0.0:8080/?application_identifier=luna&amp;&amp;organisation_code=OWryGI&amp;&amp;activity_id=c06bae49-e540-4bbb-99dc-be5518a6ef89 .
        /// Or with environment: http://127.0.0.0:8080/?application_identifier=luna&amp;&amp;organisation_code=OWryGI&amp;&amp;activity_id=c06bae49-e540-4bbb-99dc-be5518a6ef89&amp;&amp;environment_id=1
        /// </param>
        public void GetVrData(Action<VrData> responseCallback, string customServerUrl = "")
        {
            var webGlData = _webGLHelper.GetWebGlData(customServerUrl);
            StartCoroutine(LoggerHelper.GetVrData(webGlData.ServerUrl, webGlData.ApplicationIdentifier,
                webGlData.OrganisationCode, webGlData.ActivityId, webGlData.EnvironmentId, responseCallback));
        }
        
        // ------------------------------------------------ Setters section ------------------------------------------------
        
      
        /// <summary>
        /// Set custom data in json string.
        /// <br></br>
        /// Must be call before InitializeLogger()
        /// </summary>
        /// <param name="organisationCode">Organisation code</param>
        /// <seealso cref="InitializeLogger"/>
        public void SetOrganisation(string organisationCode)
        {
            _organisationCode = organisationCode;
        }

        /*
         * Set custom data in json string.
         * Must be call before InitializeLogger()
         */
        /// <summary>
        /// Set custom data in json string.
        /// <br></br>
        /// Must be call before InitializeLogger()
        /// </summary>
        /// <param name="participantId">Participant id</param>
        /// <seealso cref="InitializeLogger"/>
        public void SetParticipant(string participantId)
        {
            Debug.Log("[Vr Logger] Set participant with Id: " + participantId);
            _participantId = participantId;
            _isAnonymous = false;
        }
        
        /// <summary>
        /// Set custom data in json string.
        /// <br></br>
        /// Must be call before StartLogging()
        /// </summary>
        /// <param name="customDataJson">Custom data in Json format</param>
        /// <seealso cref="StartLogging"/>
        public void SetCustomData(string customDataJson)
        {
            try
            {
                _customData = JsonConvert.DeserializeObject<Object>(customDataJson);
            }
            catch (Exception e)
            {
                Debug.LogError("[Vr Logger] Wrong string value: " + e);
                throw;
            }
        }

        /*
         * Setting environment.
         * Can be call anytime during logging.
         */
        /// <summary>
        /// Setting environment.
        /// <br></br>
        /// Can be call anytime during logging.
        /// </summary>
        /// <param name="environment">Environment key</param>
        public void SetEnvironment(string environment)
        {
            Environment = environment;
        }
        
        /// <summary>
        /// Set record custom data in json string.
        /// <br></br>
        /// Can be call anytime during logging.
        /// </summary>
        /// <param name="eventCustomDataJson">Environment custom data in Json format</param>
        public void SetRecordCustomData(string eventCustomDataJson)
        {
            try
            {
                RecordCustomData = JsonConvert.DeserializeObject<Object>(eventCustomDataJson);
            }
            catch (Exception e)
            {
                Debug.LogError("[Vr Logger] Wrong string value: " + e);
                throw;
            }
        }
        
        /// <summary>
        /// Set event.
        /// <br></br>
        /// Can be call anytime during logging.
        /// </summary>
        /// <param name="eventKey">Event key</param>
        public void SetEvent(string eventKey)
        {
            Events.Add(eventKey);
        }
    }
}