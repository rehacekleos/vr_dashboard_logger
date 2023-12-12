using System;
using System.Collections;
using System.Collections.Generic;
using Editor.Classes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LoggerBase: MonoSingleton<LoggerBase>
    {
        public GameObject head;
        public GameObject leftHand;
        public GameObject rightHand;
        
        protected readonly List<string> Events = new List<string>();
        protected string Environment;
        protected string RecordCustomData;
        protected Activity Activity;
        
        protected LoggerHelper LoggerHelper= new LoggerHelper();
        
        private bool _isHeadNotNull;
        private bool _isLeftHandNotNull;
        private bool _isRightHandNotNull;
        private int _tick;
       

        public void Start()
        {
            _isHeadNotNull = head != null;
            _isLeftHandNotNull = leftHand != null;
            _isRightHandNotNull = rightHand != null;
        }
        
        protected IEnumerator LoggingCoroutine(float waitTime)
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

            if (Events.Count > 0)
            {
                events = Events.ToArray();
                Events.Clear();
            }

            if (!string.IsNullOrEmpty(RecordCustomData))
            {
                customData = RecordCustomData;
                RecordCustomData = null;
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

            var record = new Record(DateTime.Now, _tick, Environment, headData, leftHandData, rightHandData,
                customData, events);
            Activity.data.records.Add(record);

            _tick++;
        }
    }
}
