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
        public bool globalPositionAndRotation = true;
        
        protected readonly List<string> Events = new List<string>();
        protected string Environment;
        protected string RecordCustomData;
        protected Activity Activity;
        
        protected LoggerHelper LoggerHelper = new LoggerHelper();
        
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
                headData = globalPositionAndRotation ? PositionAndRotation.GetPositionAndRotation(head) : PositionAndRotation.GetLocalPositionAndRotation(head);
            }

            if (_isLeftHandNotNull)
            {
                leftHandData = globalPositionAndRotation ? PositionAndRotation.GetPositionAndRotation(leftHand) : PositionAndRotation.GetLocalPositionAndRotation(leftHand);
            }

            if (_isRightHandNotNull)
            {
                rightHandData = globalPositionAndRotation ? PositionAndRotation.GetPositionAndRotation(rightHand) : PositionAndRotation.GetLocalPositionAndRotation(rightHand);
            }

            var record = new Record(DateTime.Now, _tick, Environment, headData, leftHandData, rightHandData,
                customData, events);
            Activity.data.records.Add(record);

            _tick++;
        }
    }
}
