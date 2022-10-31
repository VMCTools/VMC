using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VMC.Ultilities.TimeAction
{
    public class TimedAction
    {
        public float executeTime;
        public float delayTime;
        public Action action;
        public bool ignoreTimeScale;
        public int loopTimes;

        public float countTime;
    }
}