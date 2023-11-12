using System;
using System.Collections;
using System.Collections.Generic;
using Editor.Classes;
using UnityEngine;

namespace Editor
{
    public class Logger : LoggerBase
    {
        
        private readonly string _applicationIdentifier;
        private readonly int _logRate = 300;
        private readonly string _logVersion;
        
        private string _organisationCode;
        private string _participantId;
        private bool _isAnonymous = true;
        private string _customData;
        private bool _logging;
        
        /*
         * Default Constructor with Log Rate 300 ms.
         */
        public Logger(string applicationIdentifier, string logVersion)
        {
            _applicationIdentifier = applicationIdentifier;
            _logVersion = logVersion;
        }

        /*
         * Constructor with custom Log Rate.
         */
        public Logger(string applicationIdentifier, string logVersion, int logRate)
        {
            _applicationIdentifier = applicationIdentifier;
            _logRate = logRate;
            _logVersion = logVersion;
        }

        /*
         * This method is for initialize logger.
         * Need to be call before StartLogging().
         */
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

            var vrData = new VrData(_applicationIdentifier, _logVersion, _logRate, _customData);
            Activity = new Activity(vrData, _isAnonymous, _organisationCode, participant);
        }

        
        /*
         * Starting logging VR data.
         */
        public void StartLogging(string environment)
        {
            if (_logging)
            {
                throw new Exception("Logging is active!");
            }
            
            if (Activity == null)
            {
                throw new Exception("Logger is not Initialized!");
            }

            _logging = true;
            Environment = environment;
            Activity.data.start = DateTime.Now;

            var logRateInSeconds = _logRate / 1000f;

            StartCoroutine(LoggingCoroutine(logRateInSeconds));
            Debug.Log("Logging started.");
        }
        
        /*
         * Stop logging VR data.
         */
        public void StopLogging()
        {
            if (_logging == false)
            {
                throw new Exception("Logging is not active!");
            }
            
            Activity.data.end = DateTime.Now;
            
            StopCoroutine(LoggingCoroutine(0));
            _logging = false;
            Debug.Log("Logging stopped.");
        }

        public void SendActivity(Action<bool> responseCallback)
        { 
            StartCoroutine(LoggerHelper.SendActivity(Activity, responseCallback));
        }
        
        
        // ------------------------------------------------ Setters section ------------------------------------------------
        
        /*
         * Set custom data in json string.
         * Must be call before InitializeLogger()
         */
        public void SetOrganisation(string organisationCode)
        {
            _organisationCode = organisationCode;
        }

        /*
         * Set custom data in json string.
         * Must be call before InitializeLogger()
         */
        public void SetParticipant(string participantId)
        {
            _participantId = _organisationCode;
            _isAnonymous = false;
        }

        /*
         * Set custom data in json string.
         * Must be call before StartLogging()
         */
        public void SetCustomData(string customDataJson)
        {
            _customData = customDataJson;
        }

        /*
         * Setting environment.
         * Can be call anytime during logging.
         */
        public void SetEnvironment(string environment)
        {
            Environment = environment;
        }

        /*
         * Set record custom data in json string.
         * Can be call anytime during logging.
         */
        public void SetRecordCustomData(string eventCustomDataJson)
        {
            RecordCustomData = eventCustomDataJson;
        }
        
        /*
         * Set event.
         * Can be call anytime during logging.
         */
        public void SetEvent(string eventKey)
        {
            Events.Add(eventKey);
        }
    }
}