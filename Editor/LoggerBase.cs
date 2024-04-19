using System;
using System.Collections;
using System.Collections.Generic;
using Editor.Classes;
using UnityEngine;
using Object = System.Object;

namespace Editor
{
    public class LoggerBase : MonoSingleton<LoggerBase>
    {
        [Header("Head")]
        public GameObject head;
        public bool globalHeadPositionAndRotation = true;
        
        [Header("Hands")]
        public GameObject leftHand;
        public GameObject rightHand;
        public bool globalHandsPositionAndRotation = true;
        
        [Header("Eyes")]
        public GameObject leftEye;
        public GameObject rightEye;
        public bool globalEyesPositionAndRotation = true;

        protected readonly List<string> Events = new List<string>();
        protected string Environment;
        protected Object RecordCustomData;
        protected Activity Activity;

        protected LoggerHelper LoggerHelper = new LoggerHelper();

        private bool _isHeadNotNull;
        private bool _isLeftHandNotNull;
        private bool _isRightHandNotNull;
        private bool _isLeftEyeNotNull;
        private bool _isRightEyeNotNull;
        private int _tick;


        public void Start()
        {
            _isHeadNotNull = head != null;
            _isLeftHandNotNull = leftHand != null;
            _isRightHandNotNull = rightHand != null;
            _isLeftEyeNotNull = leftEye != null;
            _isRightEyeNotNull = rightEye != null;
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
            Object customData = null;
            PositionAndRotation headData = null;
            PositionAndRotation leftHandData = null;
            PositionAndRotation rightHandData = null;
            PositionAndRotation leftEyeData = null;
            PositionAndRotation rightEyeData = null;

            if (Events.Count > 0)
            {
                events = Events.ToArray();
                Events.Clear();
            }

            if (RecordCustomData != null)
            {
                customData = RecordCustomData;
                RecordCustomData = null;
            }

            if (_isHeadNotNull)
            {
                headData = globalHeadPositionAndRotation
                    ? PositionAndRotation.GetPositionAndRotation(head)
                    : PositionAndRotation.GetLocalPositionAndRotation(head);
            }

            if (_isLeftHandNotNull)
            {
                leftHandData = globalHandsPositionAndRotation
                    ? PositionAndRotation.GetPositionAndRotation(leftHand)
                    : PositionAndRotation.GetLocalPositionAndRotation(leftHand);
            }

            if (_isRightHandNotNull)
            {
                rightHandData = globalHandsPositionAndRotation
                    ? PositionAndRotation.GetPositionAndRotation(rightHand)
                    : PositionAndRotation.GetLocalPositionAndRotation(rightHand);
            }

            if (_isLeftEyeNotNull)
            {
                leftEyeData = globalEyesPositionAndRotation
                    ? PositionAndRotation.GetPositionAndRotation(leftEye)
                    : PositionAndRotation.GetLocalPositionAndRotation(leftEye);
            }

            if (_isRightEyeNotNull)
            {
                rightEyeData = globalEyesPositionAndRotation
                    ? PositionAndRotation.GetPositionAndRotation(rightEye)
                    : PositionAndRotation.GetLocalPositionAndRotation(rightEye);
            }

            var record = new Record(DateTime.Now, _tick, Environment, headData, leftHandData, rightHandData,
                leftEyeData, rightEyeData,
                customData, events);
            Activity.data.records.Add(record);

            _tick++;
        }
    }
}