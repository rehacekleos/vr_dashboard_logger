using System;
using System.Collections;
using System.Collections.Generic;
using Editor.Classes;
using UnityEngine;

namespace Editor
{
    public class Logger : MonoBehaviour
    {

        public GameObject head;
        public GameObject leftHand;
        public GameObject rightHand;
        
        
        private string _applicationIdentifier;
        private string _organisationCode;
        private int _logRate = 300;
        private string _logVersion;
        private string _participantId;
        private bool _isAnonymous = true;
        private string _customData;
        private Activity _activity;
        private bool _logging = false;

        private int _tick = 0;
        private readonly List<string> _events = new List<string>();
        private string _environment;
        private string _recordCustomData;
        private bool _isHeadNotNull;
        private bool _isLeftHandNotNull;
        private bool _isRightHandNotNull;

        public Logger(string applicationIdentifier, string logVersion)
        {
            _applicationIdentifier = applicationIdentifier;
            _logVersion = logVersion;
        }

        public Logger(string applicationIdentifier, string logVersion, int logRate)
        {
            _applicationIdentifier = applicationIdentifier;
            _logRate = logRate;
            _logVersion = logVersion;
        }


        private void Start()
        {
            _isRightHandNotNull = rightHand != null;
            _isLeftHandNotNull = leftHand != null;
            _isHeadNotNull = head != null;
        }

        public void InitializeLogger()
        {
            if (_organisationCode == null)
            {
                throw new Exception("Organisation is not set!");
            }

            var vrData = new VrData(_applicationIdentifier, _logVersion, _logRate, _customData);
            _activity = new Activity(vrData, _isAnonymous, _organisationCode, _participantId);
        }

        public void StartLogging(string environment)
        {
            if (_activity == null)
            {
                throw new Exception("Logger is not Initialized!");
            }

            _logging = true;
            _environment = environment;
            _activity.data.start = DateTime.Now;

            var logRateInSeconds = _logRate / 1000f;

            StartCoroutine(LoggingCoroutine(logRateInSeconds));
            Debug.Log("Logging started.");
        }

        private IEnumerator LoggingCoroutine(float waitTime)
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);
                
                Logging();
            }
        }

        private void Logging()
        {
            string[] events = null;
            string customData = null;
            PositionAndRotation headData = null;
            PositionAndRotation leftHandData = null;
            PositionAndRotation rightHandData = null;
            
            if (_events.Count > 0)
            {
                events = _events.ToArray();
                _events.Clear();
            }

            if (!string.IsNullOrEmpty(_recordCustomData))
            {
                customData = _recordCustomData;
                _recordCustomData = null;
            }

            if (_isHeadNotNull)
            {
                headData = PositionAndRotation.GetPositionAndRotation(head);
            }
            
            if (_isLeftHandNotNull)
            {
                leftHandData = PositionAndRotation.GetPositionAndRotation(leftHand);
            }

            if (_isRightHandNotNull)
            {
                rightHandData = PositionAndRotation.GetPositionAndRotation(rightHand);
            }
            
            var record = new Record(DateTime.Now, _tick, _environment, headData, leftHandData, rightHandData, customData, events);
            _activity.data.records.Add(record);

            _tick++;
        }

        public void StopLogging()
        {
            if (_logging == false)
            {
                throw new Exception("Logging is not active!");
            }
            
            _activity.data.end = DateTime.Now;
            
            StopCoroutine(LoggingCoroutine(0));
            Debug.Log("Logging stopped.");
        }
        
        public void SetEvent(string eventKey)
        {
            _events.Add(eventKey);
        }
        

        public void SetOrganisation(string organisationCode)
        {
            _organisationCode = organisationCode;
        }

        public void SetParticipant(string participantId)
        {
            _participantId = _organisationCode;
            _isAnonymous = false;
        }

        /*
         * Set custom data in json string.
         */
        public void SetCustomData(string customDataJson)
        {
            _customData = customDataJson;
        }

        public void SetEnvironment(string environment)
        {
            _environment = environment;
        }

        /*
         * Set event custom data in json string.
         */
        public void SetRecordCustomData(string eventCustomDataJson)
        {
            _recordCustomData = eventCustomDataJson;
        }
    }
}